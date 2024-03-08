using Iot.Device.SocketCan;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

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
        public readonly ILogger Logger;
        public long ReadSuccessCount { get; private set; }
        public long ReadErrorCount { get; private set; }

        public CanReader(ILogger logger)
        {
            Logger = logger;
            CanDevice = new();
        }

        public void Start()
        {
            Logger.LogInformation("Start read from CAN device to memory buffer");
            CancellationToken = TokenSource.Token;
            Task.Run(() => ReadToBufferLoop(CanDevice, CancellationToken), CancellationToken);
            Task.Run(() => ParseMessages(CancellationToken), CancellationToken);
        }

        public void Stop()
        {
            Logger.LogInformation("Stop read from CAN device to memory buffer");
            TokenSource.Cancel();
        }

        private void ReadToBufferLoop(CanRaw can, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (can.TryReadFrame(buffer, out int frameLength, out CanId id))
                    {
                        if (id.Error)
                        {
                            Logger.LogWarning("Error flag set!");
                            ReadErrorCount++;
                        }
                        else
                        {
                            ReadBuffer.Enqueue(new CanInfo(id, frameLength, buffer));
                            ReadSuccessCount++;
                        }
                    }
                    else
                    {
                        Logger.LogWarning("Invalid frame received!");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Unexpected error in ReadToBufferLoop: {Message}", ex.Message);
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
                        switch (canInfo.CanId.Value)
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
                                Logger.LogInformation("Skipped unknown CAN ID {Value}", canInfo.CanId.Value);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Unexpected error in ParseMessages: {Message}", ex.Message);
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