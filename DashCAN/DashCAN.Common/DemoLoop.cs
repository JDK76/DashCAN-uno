namespace DashCAN.Common
{
    public class DemoLoop : IDataSource
    {
        private decimal DemoValue = 0;
        private decimal DemoIncrement = 0.5m;
        private readonly CancellationTokenSource TokenSource = new();
        private CancellationToken CancellationToken;

        public DataModel DataModel { get; private set; } = new();

        public long ReadSuccessCount { get; private set; }

        public long ReadErrorCount => 0;

        public void Start()
        {
            CancellationToken = TokenSource.Token;
            Task.Run(() =>
            {
                while (!CancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(30);
                    UpdateValues();
                }
            }, CancellationToken);
        }

        public void Stop()
        {
            TokenSource.Cancel();
        }

        private void UpdateValues()
        {
            DemoValue += DemoIncrement;
            if (DemoValue >= 100 || DemoValue <= 0) DemoIncrement *= -1;

            // Discrete values
            DataModel.RPM.SetValue(DemoValue * 80);
            DataModel.VehicleSpeed.SetValue(DemoValue * 2.2m);
            DataModel.FuelLevel.SetValue(DemoValue);
            DataModel.CoolantTemp.SetValue(DemoValue * 2);

            // Indicator lights
            DataModel.HighBeamLight.SetValue((DemoValue % 40 > 20));
            DataModel.IndicatorLeft.SetValue((DemoValue % 10 > 5));
            DataModel.IndicatorRight.SetValue((DemoValue % 10 > 5));

            // Warning lights
            DataModel.DoorOpenLight.SetValue(((DemoValue + 0) % 10 > 5));
            DataModel.ParkBrakeLight.SetValue(((DemoValue + 1) % 10 > 5));
            DataModel.OilPressureLight.SetValue(((DemoValue + 2) % 10 > 5));
            DataModel.BatteryLight.SetValue(((DemoValue + 3) % 10 > 5));
            DataModel.FuelLevelLight.SetValue(((DemoValue + 4) % 10 > 5));
            DataModel.CheckEngine.SetValue(((DemoValue + 5) % 10 > 5));
            DataModel.BrakeLight.SetValue(((DemoValue + 6) % 10 > 5));

            ReadSuccessCount++;
        }
    }
}
