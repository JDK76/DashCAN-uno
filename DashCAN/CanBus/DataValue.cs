namespace DashCAN.CanBus
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
                _value = value;
                LastUpdate = DateTime.UtcNow;
            }
        }

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

        internal abstract void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor);

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

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor)
        {
            throw new NotImplementedException();
        }

        public void SetValue(byte data, int offset)
        {
            IsSet = (data & (1 << offset)) != 0;
            Value = IsSet ? 1 : 0;
        }
    }

    public class RpmValue : DataValue
    {
        public override Unit Unit => Unit.RPM;

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = null)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class SpeedValue : DataValue
    {
        public override Unit Unit => Unit.Kmh;

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class VoltValue : DataValue
    {
        public override Unit Unit => Unit.Volts;

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class TemperatureValue : DataValue
    {
        public override Unit Unit => Unit.Kelvin;

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
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

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class VolumeValue : DataValue
    {
        public override Unit Unit => Unit.Litre;

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class PercentValue : DataValue
    {
        public override Unit Unit => Unit.Percent;

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 10)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class LambdaValue : DataValue
    {
        public override Unit Unit => Unit.Lambda;

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 1000)
        {
            Value = GetDecimal(bytes, offset, length, divisor);
        }
    }

    public class GearValue : DataValue
    {
        public override Unit Unit => Unit.Gear;

        public Gear Gear { get; private set; } = Gear.Unknown;

        internal override void SetValue(IEnumerable<byte> bytes, int offset, int length, decimal? divisor = 1)
        {
            var gearVal = bytes.ToArray()[offset];
            Value = ((decimal)gearVal);
            Gear = (Gear)gearVal;
        }
    }

    public enum Unit
    {
        Boolean,
        Gear,
        Kmh,
        Mph,
        Kelvin,
        Celcius,
        Fahrenheit,
        Kpa,
        KpaAbs,
        Psi,
        Lambda,
        Litre,
        UsGallon,
        Percent,
        RPM,
        Volts
    }

    public enum Gear
    {
        Unknown = -2,
        Reverse = -1,
        Neutral = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5,
        Sixth = 6
    }
}
