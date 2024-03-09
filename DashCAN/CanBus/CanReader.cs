using Iot.Device.SocketCan;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DashCAN.CanBus
{
    public class CanReader : IDisposable
    {
        private readonly ILogger Logger;
        private readonly CanRaw CanDevice = new();
        private readonly ConcurrentDictionary<uint, ConcurrentStack<CanInfo>> ReadBuffers = new();
        private readonly CancellationTokenSource TokenSource = new();
        private CancellationToken CancellationToken;

        public CanDataModel DataModel { get; private set; }
        public long ReadSuccessCount { get; private set; }
        public long ReadErrorCount { get; private set; }

        private readonly uint[] CanIdList = new uint[10] { 0x360, 0x361, 0x370, 0x372, 0x3E0, 0x3E1, 0x3E2, 0x3E4, 0x470, 0x471 };

        public CanReader(ILogger logger)
        {
            Logger = logger;
            DataModel = new(logger);

            foreach (var id in CanIdList)
            {
                ReadBuffers.TryAdd(id, new ConcurrentStack<CanInfo>());
            }
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
                    var buffer = new byte[8];
                    if (can.TryReadFrame(buffer, out int frameLength, out CanId id))
                    {
                        if (id.Error)
                        {
                            Logger.LogWarning("Error flag set!");
                            ReadErrorCount++;
                        }
                        else if (ReadBuffers.TryGetValue(id.Value, out ConcurrentStack<CanInfo>? value))
                        {
                            value.Push(new CanInfo(id, frameLength, buffer));
                            ReadSuccessCount++;
                        }
                    }
                    else
                    {
                        Logger.LogWarning("Invalid frame received!");
                        Thread.Sleep(20);
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
                    foreach (var canId in CanIdList)
                    {
                        if (ReadBuffers[canId].TryPeek(out var canInfo))
                        {
                            // Get latest from buffer and discard the rest
                            DataModel.Parse(canInfo);
                            ReadBuffers[canId].Clear();
                        }
                    }
                    Thread.Sleep(100);
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