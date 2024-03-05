using Iot.Device.SocketCan;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DashCAN.CanBus
{
    public class CanReader : IDisposable
    {
        private readonly CanRaw CanDevice;
        private readonly ConcurrentQueue<CanInfo> ReadBuffer = new();
        private readonly byte[] buffer = new byte[8];
        private readonly CancellationTokenSource TokenSource = new();
        private CancellationToken CancellationToken;
        public readonly CanDataModel DataModel = new();

        public CanReader()
        {
            CanDevice = new();
        }

        public void Start()
        {
            CancellationToken = TokenSource.Token;
            Task.Run(() => ReadToBufferLoop(CanDevice, CancellationToken), CancellationToken);
            Task.Run(() => ParseMessages(CancellationToken), CancellationToken);
        }

        public void Stop()
        {
            Debug.WriteLine("Stop read from CAN device to memory buffer");
            TokenSource.Cancel();
        }

        private void ReadToBufferLoop(CanRaw can, CancellationToken ct)
        {
            Debug.WriteLine("Start read from CAN device to memory buffer");
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (can.TryReadFrame(buffer, out int frameLength, out CanId id))
                    {
                        ReadBuffer.Enqueue(new CanInfo(id, frameLength, buffer));
                    }
                    else
                    {
                        Debug.WriteLine($"Invalid frame received!");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unexpected error in ReadToBufferLoop: " + ex.Message);
                }
            }
        }

        private void ParseMessages(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (ReadBuffer.TryDequeue(out var canInfo))
                    {
                        var messageId = canInfo.CanId.Value;
                        Debug.WriteLine($"Parsing message ID {messageId}");
                        switch (messageId)
                        {
                            case 0x360:
                                DataModel.Parse360(canInfo.Bytes);
                                break;
                            case 0x361:
                                DataModel.Parse361(canInfo.Bytes);
                                break;
                            case 0x370:
                                DataModel.Parse370(canInfo.Bytes);
                                break;
                            case 0x372:
                                DataModel.Parse372(canInfo.Bytes);
                                break;
                            case 0x3E0:
                                DataModel.Parse3E0(canInfo.Bytes);
                                break;
                            case 0x3E1:
                                DataModel.Parse3E1(canInfo.Bytes);
                                break;
                            case 0x3E2:
                                DataModel.Parse3E2(canInfo.Bytes);
                                break;
                            case 0x3E4:
                                DataModel.Parse3E4(canInfo.Bytes);
                                break;
                            case 0x470:
                                DataModel.Parse470(canInfo.Bytes);
                                break;
                            case 0x471:
                                DataModel.Parse471(canInfo.Bytes);
                                break;
                            default:
                                Debug.WriteLine($"Skipped message ID {messageId}");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unexpected error in ParseMessages: " + ex.Message);
                }
            }
        }

        public void Dispose()
        {
            CanDevice?.Dispose();
            TokenSource?.Dispose();
        }
    }
}