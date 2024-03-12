using DashCAN.Common;

namespace DashCAN.ViewModel
{
    public class DigiTacho : InstrumentValue
    {
        public DigiTacho(Unit displayUnit, DataValue dataValue) : base(displayUnit, dataValue) { }

        private int _value;
        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        protected override void SetValue(DataValue value)
        {
            Value = (int)value.ConvertUnit(DisplayUnit);
        }
    }
}