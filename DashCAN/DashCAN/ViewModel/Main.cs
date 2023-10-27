using System.Device.Gpio;

namespace DashCAN.ViewModel
{
    public class Main : ViewModelBase, IDisposable
    {
        private double Value = 0;
        private double increment = 0.5;
        private readonly GpioController GpioController;
        private const int TestPin = 18;

        public Main()
        {
            GpioController = new GpioController();
            GpioController.OpenPin(TestPin, PinMode.Output);

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

            DoorOpen.Value = ((Value + 0) % 10 > 5);
            ParkBrake.Value = ((Value + 1) % 10 > 5);
            Oil.Value = ((Value + 2) % 10 > 5);
            Battery.Value = ((Value + 3) % 10 > 5);
            Fuel.Value = ((Value + 4) % 10 > 5);
            Engine.Value = ((Value + 5) % 10 > 5);
            Brake.Value = ((Value + 6) % 10 > 5);

            SetTestPin(Value % 6 > 3);

            Value += increment;
            if (Value >= 100 || Value <= 0) increment *= -1;
        }

        private bool LastPin8State { get; set; }
        private void SetTestPin(bool state)
        {
            if (state == LastPin8State) return;
            LastPin8State = state;

            GpioController.Write(TestPin, state ? PinValue.High : PinValue.Low);
        }

        public void Dispose()
        {
            GpioController?.Dispose();
        }

        public static Brush BackgroundBrush
        {
            get { return Helpers.Brushes.SegmentBackground; }
        }

        public static Brush LitBrush
        {
            get { return Helpers.Brushes.SegmentLit; }
        }

        public DigiTacho Tachometer { get; set; } = new();

        public SevenSegmentGroup VehicleSpeed { get; set; } = new(3);

        public StackBar FuelLevel { get; set; } = new(true, false, "E", "F", 0, 100);

        public StackBar CoolantTemperature { get; set; } = new(false, true, "C", "H", 0, 150);

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
}