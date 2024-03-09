using DashCAN.CanBus;
using System.Collections.ObjectModel;

namespace DashCAN.ViewModel
{
    public class Main : ViewModelBase, IDisposable
    {
        private decimal DemoValue = 0;
        private decimal DemoIncrement = 0.5m;
        private readonly DispatcherTimer Timer;
        private readonly CanReader? CanReader;
        private readonly DataSource DataSource;
        private readonly MessageLogger Logger;

        public Main(DataSource source)
        {
            DataSource = source;
            Logger = new MessageLogger();
            Logger.MessageReceived += Logger_MessageReceived;

            if (source == DataSource.CanBus)
            {
                CanReader = new(Logger);
                CanReader.Start();
            }

            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            Timer.Start();

            Console.Clear();
            var timer2 = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500) };
            timer2.Tick += (object? sender, object e) =>
            {
                lock (Logger._MessageLock)
                {
                    Console.SetCursorPosition(0, 1);
                    Console.Write($"Temp: {CanReader.DataModel.CoolantTemp.Value:000.0} | Accel: {CanReader.DataModel.AcceleratorPedal.Value:000.0} | Fuel: {CanReader.DataModel.FuelLevel.Value:000.0} | RPM: {(int)CanReader.DataModel.RPM.Value:00000}");
                }
            };
            timer2.Start();

            Logger.LogInformation("Startup for {source}", source);
        }

        private void Logger_MessageReceived(object? sender, MessageEventArgs e)
        {
            Messages.Add(e.Message);
        }

        private void Timer_Tick(object? sender, object e)
        {
            if (DataSource == DataSource.Demo)
            {
                UpdateDemo();
                DemoValue += DemoIncrement;
                if (DemoValue >= 100 || DemoValue <= 0)
                {
                    DemoIncrement *= -1;
                    Logger.Log(LogLevel.Information, "Reverse!");
                }
            }
            else if (DataSource == DataSource.CanBus)
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

        public ObservableCollection<Tuple<LogLevel, string>> Messages { get; set; } = new();
    }

    public enum DataSource
    {
        Demo,
        CanBus
    }

    public class MessageLogger : ILogger
    {
        public List<Tuple<LogLevel, string>> Messages { get; private set; } = new();
        public event EventHandler<MessageEventArgs>? MessageReceived;
        public object _MessageLock = new();

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = Tuple.Create(logLevel, $"{formatter(state, exception)}");
            Messages.Add(message);
            MessageReceived?.Invoke(this, new MessageEventArgs(message));
            System.Diagnostics.Debug.WriteLine(message.Item2);

            lock (_MessageLock)
            {
                if (logLevel == LogLevel.Error)
                {
                    Console.SetCursorPosition(0, 7);
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (logLevel == LogLevel.Warning)
                {
                    Console.SetCursorPosition(0, 6);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else if (logLevel == LogLevel.Information)
                {
                    Console.SetCursorPosition(0, 5);
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (logLevel == LogLevel.Debug)
                {
                    Console.SetCursorPosition(0, 4);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    Console.SetCursorPosition(0, 3);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write(message.Item2);
                Console.ResetColor();
            }
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(Tuple<LogLevel, string> message)
        {
            Message = message;
        }

        public Tuple<LogLevel, string> Message { get; private set; }
    }
}