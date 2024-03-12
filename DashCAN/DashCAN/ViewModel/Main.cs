using DashCAN.CanBus;
using DashCAN.Common;
using System.Collections.ObjectModel;

namespace DashCAN.ViewModel
{
    public class Main : ViewModelBase, IDisposable
    {
        private readonly IDataSource DataSource;
        private readonly DataSource DataSourceType;
        private readonly MessageLogger Logger;
        private bool HasConsole { get; set; }

        public Main(DataSource sourceType)
        {
#if HAS_UNO
            HasConsole = true;
#endif
            DataSourceType = sourceType;
            Logger = new MessageLogger();
            Logger.MessageReceived += Logger_MessageReceived;

            if (DataSourceType == Common.DataSource.CanBus)
            {
                DataSource = new CanReader(Logger);
            }
            else
            {
                DataSource = new DemoLoop();
            }

            if (HasConsole)
            {
                Console.Clear();
                var consoleTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500) };
                consoleTimer.Tick += (object? sender, object e) =>
                {
                    lock (Logger._MessageLock)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.Write($"Temp: {DataSource.DataModel.CoolantTemp.Value:000.0} | Accel: {DataSource.DataModel.AcceleratorPedal.Value:000.0} | Fuel: {DataSource.DataModel.FuelLevel.Value:000.0} | RPM: {(int)DataSource.DataModel.RPM.Value:00000}");
                    }
                };
                consoleTimer.Start();
            }

            Logger.LogInformation("Startup for {source}", DataSourceType);
            Start();
        }

        private void Start()
        {
            AnalogTacho = new(Unit.RPM, DataSource.DataModel.RPM);
            Tachometer = new(Unit.RPM, DataSource.DataModel.RPM);
            AnalogSpeed = new(Unit.Kmh, DataSource.DataModel.VehicleSpeed);
            VehicleSpeed = new(Unit.Kmh, DataSource.DataModel.VehicleSpeed, 3);
            FuelLevel = new(Unit.Litre, DataSource.DataModel.FuelLevel, true, false, "E", "F", 0, 65);
            CoolantTemperature = new(Unit.Celcius, DataSource.DataModel.CoolantTemp, false, true, "C", "H", 0, 150);

            DoorOpen = new Warning(WarningType.DoorOpen, DataSource.DataModel.DoorOpenLight);
            ParkBrake = new Warning(WarningType.ParkBrake, DataSource.DataModel.ParkBrakeLight);
            Oil = new Warning(WarningType.Oil, DataSource.DataModel.OilPressureLight);
            Battery = new Warning(WarningType.Battery, DataSource.DataModel.BatteryLight);
            Fuel = new Warning(WarningType.Fuel, DataSource.DataModel.FuelLevelLight);
            Engine = new Warning(WarningType.Engine, DataSource.DataModel.CheckEngine);
            Brake = new Warning(WarningType.Brake, DataSource.DataModel.BrakeLight);

            HighBeam = new(IndicatorType.HighBeam, DataSource.DataModel.HighBeamLight);
            Left = new(IndicatorType.Left, DataSource.DataModel.IndicatorLeft);
            Right = new(IndicatorType.Right, DataSource.DataModel.IndicatorRight);

            DataSource.Start();
        }

        private void Logger_MessageReceived(object? sender, MessageEventArgs e)
        {
            Messages.Add(e.Message);
        }

        public void Dispose()
        {
            DataSource?.Stop();
            (DataSource as IDisposable)?.Dispose();
        }

        public static Brush BackgroundBrush
        {
            get { return Helpers.Brushes.SegmentBackground; }
        }

        public static Brush LitBrush
        {
            get { return Helpers.Brushes.SegmentLit; }
        }

        public Dial AnalogTacho { get; private set; }

        public DigiTacho Tachometer { get; private set; }

        public Dial AnalogSpeed { get; private set; }

        public SevenSegmentGroup VehicleSpeed { get; private set; }

        public StackBar FuelLevel { get; private set; }

        public StackBar CoolantTemperature { get; private set; }

        public Indicator HighBeam { get; private set; } 

        public Indicator Left { get; private set; } 

        public Indicator Right { get; private set; } 

        public Warning DoorOpen { get; private set; } 

        public Warning ParkBrake { get; private set; } 

        public Warning Oil { get; private set; } 

        public Warning Battery { get; private set; } 

        public Warning Fuel { get; private set; } 

        public Warning Engine { get; private set; } 

        public Warning Brake { get; private set; } 

        public ObservableCollection<Tuple<LogLevel, string>> Messages { get; private set; } = new();
    }

    public class MessageLogger : ILogger
    {
        public MessageLogger()
        {
#if HAS_UNO
            HasConsole = true;
#endif
        }

        public List<Tuple<LogLevel, string>> Messages { get; private set; } = new();
        public event EventHandler<MessageEventArgs>? MessageReceived;
        public object _MessageLock = new();
        private bool HasConsole { get; set; }

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

            if (HasConsole)
            {
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