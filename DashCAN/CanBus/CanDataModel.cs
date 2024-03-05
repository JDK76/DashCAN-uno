namespace DashCAN.CanBus
{
    public class CanDataModel
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

        public void Parse360(byte[] data)
        {
            RPM.SetValue(data, 0, 2);
            ManifoldPressure.SetValue(data, 2, 2);
            ThrottlePosition.SetValue(data, 4, 2);
            CoolantPressure.SetValue(data, 6, 2);
        }

        public void Parse361(byte[] data)
        {
            FuelPressure.SetValue(data, 2, 2);
            OilPressure.SetValue(data, 2, 2);
            EngineDemand.SetValue(data, 4, 2);
            WastegatePressure.SetValue(data, 6, 2);
        }

        public void Parse370(byte[] data)
        {
            VehicleSpeed.SetValue(data, 0, 2);
            TargetBoost.SetValue(data, 4, 2);
            BarometricPressure.SetValue(data, 6, 2);
        }

        public void Parse372(byte[] data)
        {
            BatteryVoltage.SetValue(data, 0, 2);
        }

        public void Parse3E0(byte[] data)
        {
            CoolantPressure.SetValue(data, 0, 2);
            AirTemp.SetValue(data, 2, 2);
            FuelTemp.SetValue(data, 4, 2);
            OilTemp.SetValue(data, 6, 2);
        }

        public void Parse3E1(byte[] data)
        {
            FuelComposition.SetValue(data, 4, 2);
        }

        public void Parse3E2(byte[] data)
        {
            FuelLevel.SetValue(data, 4, 2);
        }

        public void Parse3E4(byte[] data)
        {
            NeutralSwitch.SetValue(data[1], 7);
            ReverseSwitch.SetValue(data[1], 6);
            GearSwitch.SetValue(data[1], 5);
            DecelCut.SetValue(data[1], 4);
            TransThrottle.SetValue(data[1], 3);
            BrakePedal.SetValue(data[1], 2);
            ClutchPedal.SetValue(data[1], 1);
            OilPressureLight.SetValue(data[1], 0);
            ThermoFan4.SetValue(data[3], 3);
            ThermoFan3.SetValue(data[3], 2);
            ThermoFan2.SetValue(data[3], 1);
            ThermoFan1.SetValue(data[3], 0);
            CheckEngine.SetValue(data[7], 7);
            BatteryLight.SetValue(data[7], 6);
        }

        public void Parse470(byte[] data)
        {
            WidebandOverall.SetValue(data, 0, 2);
            WidebandBank1.SetValue(data, 2, 2);
            WidebandBank2.SetValue(data, 4, 2);
            SelectedGear.SetValue(data, 7, 1);
        }

        public void Parse471(byte[] data)
        {
            AcceleratorPedal.SetValue(data, 2, 2);
        }
    }
}
