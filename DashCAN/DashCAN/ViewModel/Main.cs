using DashCAN.CanBus;

namespace DashCAN.ViewModel
{
    public class Main : ViewModelBase, IDisposable
    {
        private decimal DemoValue = 0;
        private decimal DemoIncrement = 0.5m;
        private readonly DispatcherTimer Timer;
        private readonly CanReader? CanReader;
        private readonly DataSource DataSource;

        public Main(DataSource source)
        {
            DataSource = source;

            if (source == DataSource.CanBus)
            {
                CanReader = new();
                CanReader.Start();
            }

            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            Timer.Start();
        }

        private void Timer_Tick(object? sender, object e)
        {
            if (DataSource == DataSource.Demo)
            {
                UpdateDemo();
                DemoValue += DemoIncrement;
                if (DemoValue >= 100 || DemoValue <= 0) DemoIncrement *= -1;
            }
            else if (DataSource == DataSource.Demo)
            {
                UpdateCAN();
            }
        }

        private void UpdateCAN()
        {
            if (CanReader == null) return;

            AnalogTacho.SetValue(CanReader.DataModel.RPM);
            Tachometer.SetValue(CanReader.DataModel.RPM);
            AnalogSpeed.SetValue(CanReader.DataModel.VehicleSpeed);
            VehicleSpeed.SetValue(CanReader.DataModel.VehicleSpeed);
            FuelLevel.SetValue(CanReader.DataModel.FuelLevel);
            CoolantTemperature.SetValue(CanReader.DataModel.CoolantTemp);
            //HighBeam.SetValue(CanReader.DataModel.HighBeam);
            //Left.SetValue(CanReader.DataModel.LeftIndicator);
            //Right.SetValue(CanReader.DataModel.RightIndicator);

            Engine.SetValue(CanReader.DataModel.CheckEngine);
            Oil.SetValue(CanReader.DataModel.OilPressureLight);
            Battery.SetValue(CanReader.DataModel.BatteryLight);
            //DoorOpen.SetValue(CanReader.DataModel.DoorOpenLight);
            //ParkBrake.SetValue(CanReader.DataModel.ParkBrakeLight);
            //Fuel.SetValue(CanReader.DataModel.FuelWarningLight);
            //Brake.SetValue(CanReader.DataModel.BrakeWarningLight);
        }

        private void UpdateDemo()
        {
            AnalogTacho.Value = DemoValue * 80;
            Tachometer.Value = (int)DemoValue * 80;
            AnalogSpeed.Value = DemoValue * 2.2m;
            VehicleSpeed.Value = (int)(DemoValue * 2.0m);
            FuelLevel.Value = DemoValue;
            CoolantTemperature.Value = DemoValue * 2;
            HighBeam.Value = (DemoValue % 40 > 20);
            Left.Value = (DemoValue % 10 > 5);
            Right.Value = (DemoValue % 10 > 5);

            DoorOpen.Value = ((DemoValue + 0) % 10 > 5);
            ParkBrake.Value = ((DemoValue + 1) % 10 > 5);
            Oil.Value = ((DemoValue + 2) % 10 > 5);
            Battery.Value = ((DemoValue + 3) % 10 > 5);
            Fuel.Value = ((DemoValue + 4) % 10 > 5);
            Engine.Value = ((DemoValue + 5) % 10 > 5);
            Brake.Value = ((DemoValue + 6) % 10 > 5);
        }

        public void Dispose()
        {
            Timer?.Stop();
            CanReader?.Stop();
            CanReader?.Dispose();
        }

        public static Brush BackgroundBrush
        {
            get { return Helpers.Brushes.SegmentBackground; }
        }

        public static Brush LitBrush
        {
            get { return Helpers.Brushes.SegmentLit; }
        }

        public Dial AnalogTacho { get; set; } = new(Unit.RPM);

        public DigiTacho Tachometer { get; set; } = new(Unit.RPM);

        public Dial AnalogSpeed { get; set; } = new(Unit.Kmh);

        public SevenSegmentGroup VehicleSpeed { get; set; } = new(Unit.Kmh, 3);

        public StackBar FuelLevel { get; set; } = new(Unit.Litre, true, false, "E", "F", 0, 65);

        public StackBar CoolantTemperature { get; set; } = new(Unit.Celcius, false, true, "C", "H", 0, 150);

        public Indicator HighBeam { get; set; } = new(IndicatorType.HighBeam);

        public Indicator Left { get; set; } = new(IndicatorType.Left);

        public Indicator Right { get; set; } = new(IndicatorType.Right);

        public Warning DoorOpen { get; set; } = new Warning(WarningType.DoorOpen);

        public Warning ParkBrake { get; set; } = new Warning(WarningType.ParkBrake);

        public Warning Oil { get; set; } = new Warning(WarningType.Oil);

        public Warning Battery { get; set; } = new Warning(WarningType.Battery);

        public Warning Fuel { get; set; } = new Warning(WarningType.Fuel);

        public Warning Engine { get; set; } = new Warning(WarningType.Engine);

        public Warning Brake { get; set; } = new Warning(WarningType.Brake);
    }

    public enum DataSource
    {
        Demo,
        CanBus
    }
}