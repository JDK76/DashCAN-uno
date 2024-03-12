namespace DashCAN.Common
{
    public interface IDataSource
    {
        public DataModel DataModel { get; }
        public long ReadSuccessCount { get; }
        public long ReadErrorCount { get; }
        public void Start();
        public void Stop();

    }
}
