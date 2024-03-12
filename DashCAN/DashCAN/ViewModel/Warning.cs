using DashCAN.Common;

namespace DashCAN.ViewModel
{
    public class Warning : InstrumentValue
    {
        public Warning(WarningType type, DataValue dataValue) : base(Unit.Boolean, dataValue)
        {
            Type = type;
        }

        private WarningType _type;
        public WarningType Type
        {
            get { return _type; }
            private set { SetProperty(ref _type, value); }
        }

        private bool _value;
        public bool Value
        {
            get { return _value; }
            private set { if (SetProperty(ref _value, value)) OnPropertyChanged(new string[] { nameof(MainColour) }); }
        }

        protected override void SetValue(DataValue value)
        {
            var boolValue = (value as BoolValue)?.IsSet;
            if (boolValue.HasValue) Value = boolValue.Value;
        }

        public SolidColorBrush MainColour
        {
            get
            {
                switch (Type)
                {
                    case WarningType.DoorOpen:
                    case WarningType.ParkBrake:
                    case WarningType.Oil:
                    case WarningType.Battery:
                    case WarningType.Brake:
                        return Value ? Helpers.Brushes.WarningLitRed : Helpers.Brushes.WarningUnlit;
                    case WarningType.Fuel:
                    case WarningType.Engine:
                        return Value ? Helpers.Brushes.WarningLitOrange : Helpers.Brushes.WarningUnlit;
                    default:
                        return Helpers.Brushes.Transparent;
                }
            }
        }
    }
}
