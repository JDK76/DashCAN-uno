namespace DashCAN.Common
{
    public class DataModel
    {
        // 0x360
        public RpmValue RPM { get; set; } = new();
        public PressureValue ManifoldPressure { get; set; } = new(true);
        public PercentValue ThrottlePosition { get; set; } = new();
        public PressureValue CoolantPressure { get; set; } = new();

        // 0x361
        public PressureValue FuelPressure { get; set; } = new();
        public PressureValue OilPressure { get; set; } = new();
        public PercentValue EngineDemand { get; set; } = new();
        public PressureValue WastegatePressure { get; set; } = new();

        // 0x370
        public SpeedValue VehicleSpeed { get; set; } = new();

        // 0x372
        public VoltValue BatteryVoltage { get; set; } = new();
        public PressureValue TargetBoost { get; set; } = new();
        public PressureValue BarometricPressure { get; set; } = new(true);

        // 0x3E0
        public TemperatureValue CoolantTemp { get; set; } = new();
        public TemperatureValue AirTemp { get; set; } = new();
        public TemperatureValue FuelTemp { get; set; } = new();
        public TemperatureValue OilTemp { get; set; } = new();

        // 0x3E1
        public PercentValue FuelComposition { get; set; } = new();

        // 0x3E2
        public VolumeValue FuelLevel { get; set; } = new();

        // 0x3E4
        public BoolValue NeutralSwitch { get; set; } = new();
        public BoolValue ReverseSwitch { get; set; } = new();
        public BoolValue GearSwitch { get; set; } = new();
        public BoolValue DecelCut { get; set; } = new();
        public BoolValue TransThrottle { get; set; } = new();
        public BoolValue BrakePedal { get; set; } = new();
        public BoolValue ClutchPedal { get; set; } = new();
        public BoolValue OilPressureLight { get; set; } = new();
        public BoolValue ThermoFan1 { get; set; } = new();
        public BoolValue ThermoFan2 { get; set; } = new();
        public BoolValue ThermoFan3 { get; set; } = new();
        public BoolValue ThermoFan4 { get; set; } = new();
        public BoolValue CheckEngine { get; set; } = new();
        public BoolValue BatteryLight { get; set; } = new();

        // 0x470
        public LambdaValue WidebandOverall { get; set; } = new();
        public LambdaValue WidebandBank1 { get; set; } = new();
        public LambdaValue WidebandBank2 { get; set; } = new();
        public GearValue SelectedGear { get; set; } = new();

        // 0x471
        public PercentValue AcceleratorPedal { get; set; } = new();

        // Others
        public BoolValue HighBeamLight { get; set; } = new();
        public BoolValue IndicatorLeft { get; set; } = new();
        public BoolValue IndicatorRight { get; set; } = new();
        public BoolValue DoorOpenLight { get; set; } = new();
        public BoolValue ParkBrakeLight { get; set; } = new();
        public BoolValue FuelLevelLight { get; set; } = new();
        public BoolValue BrakeLight { get; set; } = new();
    }
}
