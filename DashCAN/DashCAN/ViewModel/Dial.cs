using DashCAN.Common;

namespace DashCAN.ViewModel
{
    public class Dial : InstrumentValue
    {
        public Dial(Unit displayUnit, DataValue dataValue) : base(displayUnit, dataValue) { }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        private decimal _value;
        public decimal Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        protected override void SetValue(DataValue value)
        {
            Value = value.ConvertUnit(DisplayUnit);
        }
    }
}
