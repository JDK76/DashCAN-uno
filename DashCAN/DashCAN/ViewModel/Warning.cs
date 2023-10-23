namespace DashCAN.ViewModel
{
    public class Warning : ViewModelBase
    {
        public Warning(WarningType type)
        {
            Type = type;
        }

        private WarningType _type;
        public WarningType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private bool _value;
        public bool Value
        {
            get { return _value; }
            set { if (SetProperty(ref _value, value)) OnPropertyChanged(new string[] { nameof(MainColour) }); }
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
