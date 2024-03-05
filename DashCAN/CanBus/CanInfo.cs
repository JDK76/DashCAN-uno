using Iot.Device.SocketCan;

namespace DashCAN.CanBus
{
    public class CanInfo
    {
        public CanInfo(CanId canid, int frameLen, byte[] bytes)
        {
            CanId = canid;
            FrameLength = frameLen;
            Bytes = bytes;
        }
        public byte[] Bytes { get; private set; }
        public CanId CanId { get; private set; }
        public int FrameLength { get; private set; }
    }
}
