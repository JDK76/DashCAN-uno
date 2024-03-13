using DashCAN.Common;
using Iot.Device.SocketCan;
using Microsoft.Extensions.Logging;

namespace DashCAN.CanBus
{
    public class CanInfo
    {
        public CanInfo(CanId canid, int frameLen, byte[] bytes, ILogger logger)
        {
            CanId = canid;
            FrameLength = frameLen;
            Bytes = bytes;
            Logger = logger;
        }

        private ILogger Logger { get; set; }
        public byte[] Bytes { get; private set; }
        public CanId CanId { get; private set; }
        public int FrameLength { get; private set; }

        public void Parse(DataModel model)
        {
            var canId = CanId.Value;

            if (Bytes == null || Bytes.Length == 0)
            {
                Logger.LogWarning("Data was empty for CAN ID {canId}", canId);
                return;
            }
            else if (Bytes.Length != 8)
            {
                Logger.LogWarning("Unexpected data length {Length} for CAN ID {canId}", Bytes.Length, canId);
                return;
            }

            var methodName = $"Parse{canId:X3}";
            var method = GetType().GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, new Type[] { typeof(DataModel) });
            if (method == null) Logger.LogError("No method '{methodName}' in {type}", methodName, GetType().Name);
            method?.Invoke(this, new object[] { model });
        }

        private void Parse360(DataModel model)
        {
            model.RPM.SetValue(Bytes, 0, 2);
            model.ManifoldPressure.SetValue(Bytes, 2, 2);
            model.ThrottlePosition.SetValue(Bytes, 4, 2);
            model.CoolantPressure.SetValue(Bytes, 6, 2);
        }

        private void Parse361(DataModel model)
        {
            model.FuelPressure.SetValue(Bytes, 2, 2);
            model.OilPressure.SetValue(Bytes, 2, 2);
            model.EngineDemand.SetValue(Bytes, 4, 2);
            model.WastegatePressure.SetValue(Bytes, 6, 2);
        }

        private void Parse370(DataModel model)
        {
            model.VehicleSpeed.SetValue(Bytes, 0, 2);
            model.TargetBoost.SetValue(Bytes, 4, 2);
            model.BarometricPressure.SetValue(Bytes, 6, 2);
        }

        private void Parse372(DataModel model)
        {
            model.BatteryVoltage.SetValue(Bytes, 0, 2);
        }

        private void Parse3E0(DataModel model)
        {
            model.CoolantTemp.SetValue(Bytes, 0, 2);
            model.AirTemp.SetValue(Bytes, 2, 2);
            model.FuelTemp.SetValue(Bytes, 4, 2);
            model.OilTemp.SetValue(Bytes, 6, 2);
        }

        private void Parse3E1(DataModel model)
        {
            model.FuelComposition.SetValue(Bytes, 4, 2);
        }

        private void Parse3E2(DataModel model)
        {
            model.FuelLevel.SetValue(Bytes, 0, 2);
        }

        private void Parse3E4(DataModel model)
        {
            model.NeutralSwitch.SetValue(Bytes[1], 7);
            model.ReverseSwitch.SetValue(Bytes[1], 6);
            model.GearSwitch.SetValue(Bytes[1], 5);
            model.DecelCut.SetValue(Bytes[1], 4);
            model.TransThrottle.SetValue(Bytes[1], 3);
            model.BrakePedal.SetValue(Bytes[1], 2);
            model.ClutchPedal.SetValue(Bytes[1], 1);
            model.OilPressureLight.SetValue(Bytes[1], 0);
            model.ThermoFan4.SetValue(Bytes[3], 3);
            model.ThermoFan3.SetValue(Bytes[3], 2);
            model.ThermoFan2.SetValue(Bytes[3], 1);
            model.ThermoFan1.SetValue(Bytes[3], 0);
            model.CheckEngine.SetValue(Bytes[7], 7);
            model.BatteryLight.SetValue(Bytes[7], 6);
        }

        private void Parse470(DataModel model)
        {
            model.WidebandOverall.SetValue(Bytes, 0, 2);
            model.WidebandBank1.SetValue(Bytes, 2, 2);
            model.WidebandBank2.SetValue(Bytes, 4, 2);
            model.SelectedGear.SetValue(Bytes, 7, 1);
        }

        private void Parse471(DataModel model)
        {
            model.AcceleratorPedal.SetValue(Bytes, 2, 2);
        }
    }
}
