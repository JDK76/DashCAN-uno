using DashCAN.CanBus;

namespace DashCAN.ViewModel
{
    public class Dial : InstrumentValue
    {
        public Dial(Unit displayUnit) : base(displayUnit) { }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        private decimal _value;
        public decimal Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public override void SetValue(DataValue value)
        {
            Value = value.ConvertUnit(DisplayUnit);
        }
    }
}
