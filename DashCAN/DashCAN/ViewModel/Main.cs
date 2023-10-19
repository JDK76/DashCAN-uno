namespace DashCAN.ViewModel
{
    public class Main : ViewModelBase
    {
        private double Value = 0;
        private double increment = 0.5;

        public Main()
        {
            var timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            timer.Start();
        }

        private void Timer_Tick(object? sender, object e)
        {
            Tachometer.Value = (int)Value * 100;
            VehicleSpeed.Value = (int)(Value * 2.0);
            FuelLevel.Value = Value;
            CoolantTemperature.Value = Value * 2;
            HighBeam.Value = (Value % 40 > 20);
            Left.Value = (Value % 10 > 5);
            Right.Value = (Value % 10 > 5);

            Value += increment;
            if (Value >= 100 || Value <= 0) increment *= -1;
        }

        public static Brush BackgroundBrush
        {
            get { return Helpers.Brushes.SegmentBackgroundColour; }
        }

        public static Brush LitBrush
        {
            get { return Helpers.Brushes.SegmentLitColour; }
        }

        public DigiTacho Tachometer { get; set; } = new();

        public SevenSegmentGroup VehicleSpeed { get; set; } = new(3);

        public StackBar FuelLevel { get; set; } = new(true, false, "E", "F", 0, 100);

        public StackBar CoolantTemperature { get; set; } = new(false, true, "C", "H", 0, 150);

        public Indicator HighBeam { get; set; } = new(IndicatorType.HighBeam);

        public Indicator Left { get; set; } = new(IndicatorType.Left);

        public Indicator Right { get; set; } = new(IndicatorType.Right);
    }
}