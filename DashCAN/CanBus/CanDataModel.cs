using Microsoft.Extensions.Logging;

namespace DashCAN.CanBus
{
    public class CanDataModel
    {
        public CanDataModel(ILogger logger)
        {
            Logger = logger;
        }

        private ILogger Logger { get; set; }

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

        public void Parse(CanInfo canInfo)
        {
            var canId = canInfo.CanId.Value;
            var data = canInfo.Bytes;

            if (canInfo.Bytes == null ||  data.Length == 0)
            {
                Logger.LogWarning("Data was empty for CAN ID {canId}", canId);
                return;
            }
            else if (canInfo.Bytes.Length != 8)
            {
                Logger.LogWarning("Unexpected data length {Length} for CAN ID {canId}", data.Length, canId);
                return;
            }

            var methodName = $"Parse{canId:X3}";
            var method = typeof(CanDataModel).GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, new Type[] { typeof(CanInfo) });
            if (method == null) Logger.LogError("No method '{methodName}' in {type}", methodName, nameof(CanDataModel));
            method?.Invoke(this, new object[] { canInfo });
        }

        private void Parse360(CanInfo canInfo)
        {
            RPM.SetValue(canInfo.Bytes, 0, 2);
            ManifoldPressure.SetValue(canInfo.Bytes, 2, 2);
            ThrottlePosition.SetValue(canInfo.Bytes, 4, 2);
            CoolantPressure.SetValue(canInfo.Bytes, 6, 2);
        }

        private void Parse361(CanInfo canInfo)
        {
            FuelPressure.SetValue(canInfo.Bytes, 2, 2);
            OilPressure.SetValue(canInfo.Bytes, 2, 2);
            EngineDemand.SetValue(canInfo.Bytes, 4, 2);
            WastegatePressure.SetValue(canInfo.Bytes, 6, 2);
        }

        private void Parse370(CanInfo canInfo)
        {
            VehicleSpeed.SetValue(canInfo.Bytes, 0, 2);
            TargetBoost.SetValue(canInfo.Bytes, 4, 2);
            BarometricPressure.SetValue(canInfo.Bytes, 6, 2);
        }

        private void Parse372(CanInfo canInfo)
        {
            BatteryVoltage.SetValue(canInfo.Bytes, 0, 2);
        }

        private void Parse3E0(CanInfo canInfo)
        {
            CoolantTemp.SetValue(canInfo.Bytes, 0, 2);
            AirTemp.SetValue(canInfo.Bytes, 2, 2);
            FuelTemp.SetValue(canInfo.Bytes, 4, 2);
            OilTemp.SetValue(canInfo.Bytes, 6, 2);
        }

        private void Parse3E1(CanInfo canInfo)
        {
            FuelComposition.SetValue(canInfo.Bytes, 4, 2);
        }

        private void Parse3E2(CanInfo canInfo)
        {
            FuelLevel.SetValue(canInfo.Bytes, 0, 2);
        }

        private void Parse3E4(CanInfo canInfo)
        {
            NeutralSwitch.SetValue(canInfo.Bytes[1], 7);
            ReverseSwitch.SetValue(canInfo.Bytes[1], 6);
            GearSwitch.SetValue(canInfo.Bytes[1], 5);
            DecelCut.SetValue(canInfo.Bytes[1], 4);
            TransThrottle.SetValue(canInfo.Bytes[1], 3);
            BrakePedal.SetValue(canInfo.Bytes[1], 2);
            ClutchPedal.SetValue(canInfo.Bytes[1], 1);
            OilPressureLight.SetValue(canInfo.Bytes[1], 0);
            ThermoFan4.SetValue(canInfo.Bytes[3], 3);
            ThermoFan3.SetValue(canInfo.Bytes[3], 2);
            ThermoFan2.SetValue(canInfo.Bytes[3], 1);
            ThermoFan1.SetValue(canInfo.Bytes[3], 0);
            CheckEngine.SetValue(canInfo.Bytes[7], 7);
            BatteryLight.SetValue(canInfo.Bytes[7], 6);
        }

        private void Parse470(CanInfo canInfo)
        {
            WidebandOverall.SetValue(canInfo.Bytes, 0, 2);
            WidebandBank1.SetValue(canInfo.Bytes, 2, 2);
            WidebandBank2.SetValue(canInfo.Bytes, 4, 2);
            SelectedGear.SetValue(canInfo.Bytes, 7, 1);
        }

        private void Parse471(CanInfo canInfo)
        {
            AcceleratorPedal.SetValue(canInfo.Bytes, 2, 2);
        }
    }
}
