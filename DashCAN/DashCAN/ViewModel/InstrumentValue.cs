using DashCAN.CanBus;

namespace DashCAN.ViewModel
{
    public abstract class InstrumentValue : ViewModelBase
    {
        public InstrumentValue(Unit displayUnit)
        {
            DisplayUnit = displayUnit;
        }

        public Unit DisplayUnit { get; private set; }

        public abstract void SetValue(DataValue value);
    }
}
