using DashCAN.CanBus;

namespace DashCAN.ViewModel
{
    public class DigiTacho : InstrumentValue
    {
        public DigiTacho(Unit displayUnit) : base(displayUnit) { }

        private int _value;
        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public override void SetValue(DataValue value)
        {
            Value = (int)value.ConvertUnit(DisplayUnit);
        }
    }
}