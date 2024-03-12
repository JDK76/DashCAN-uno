namespace DashCAN.Common
{
    public abstract class DataValue
    {
        private decimal _value;
        public decimal Value
        {
            get
            {
                return _value;
            }
            internal set
            {
                LastUpdate = DateTime.UtcNow;
                if (_value != value)
                {
                    _value = value;
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? ValueChanged;

        public decimal ConvertUnit(Unit targetUnit)
        {
            var convertedValue = UnitHelper.ConvertUnit(Unit, targetUnit, Value);
            if (convertedValue.HasValue) return convertedValue.Value;
            throw new ArgumentException($"Cannot convert {Unit} to {targetUnit}", nameof(targetUnit));
        }

        public override string ToString()
        {
            return $"{Value:0.0} {Unit}";
        }

        public DateTime? LastUpdate { get; private set; }

        public abstract Unit Unit { get; }

        public abstract void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor);

        public void SetValue(decimal value)
        {
            Value = value;
        }

        protected static uint GetUInt(IEnumerable<byte> bytes, int offset, int length)
        {
            if (offset + length > bytes.Count()) return 0;
            if (length == 2)
                return BitConverter.ToUInt16(bytes.Skip(offset).Take(length).Reverse().ToArray());
            else if (length == 4)
                return BitConverter.ToUInt32(bytes.Skip(offset).Take(length).Reverse().ToArray());
            else
                return 0;
        }

        protected static decimal GetDecimal(IEnumerable<byte> bytes, int offset, int length, decimal? divisor)
        {
            return ((decimal)GetUInt(bytes, offset, length)) / (divisor ?? 1);
        }
    }

    public class BoolValue : DataValue
    {
        public override Unit Unit => Unit.Boolean;

        public bool IsSet { get; private set; }

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor)
        {
            throw new NotImplementedException();
        }

        public void SetValue(byte data, int offset)
        {
            SetValue((data & (1 << offset)) != 0);
        }

        public void SetValue(bool value)
        {
            IsSet = value;
            Value = value ? 1 : 0;
        }
    }

    public class RpmValue : DataValue
    {
        public override Unit Unit => Unit.RPM;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = null)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class SpeedValue : DataValue
    {
        public override Unit Unit => Unit.Kmh;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class VoltValue : DataValue
    {
        public override Unit Unit => Unit.Volts;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class TemperatureValue : DataValue
    {
        public override Unit Unit => Unit.Kelvin;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class PressureValue : DataValue
    {
        public PressureValue()
        {
            IsAbsolute = false;
        }

        public PressureValue(bool isAbsolute)
        {
            IsAbsolute = isAbsolute;
        }

        public bool IsAbsolute { get; private set; }
        public override Unit Unit => IsAbsolute ? Unit.KpaAbs : Unit.Kpa;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class VolumeValue : DataValue
    {
        public override Unit Unit => Unit.Litre;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class PercentValue : DataValue
    {
        public override Unit Unit => Unit.Percent;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class LambdaValue : DataValue
    {
        public override Unit Unit => Unit.Lambda;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 1000)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class GearValue : DataValue
    {
        public override Unit Unit => Unit.Gear;

        public Gear Gear { get; private set; } = Gear.Unknown;

        public override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 1)
        {
            var gearVal = bytes.ToArray()[offset];
            Value = ((decimal)gearVal);
            Gear = (Gear)gearVal;
        }
    }
}
