using Microsoft.UI;
using Windows.UI;

namespace DashCAN.ViewModel
{
    public class Indicator : ViewModelBase
    {
        public Indicator(IndicatorType type)
        {
            Type = type;
        }

        private IndicatorType _type = IndicatorType.None;
        public IndicatorType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private bool _value;
        public bool Value
        {
            get { return _value; }
            set { if (SetProperty(ref _value, value)) OnPropertyChanged(new string[] { "BackgroundBrush", "LeftGlowColour", "LeftBrush", "RightGlowColour", "RightBrush" }); }
        }

        public static Color HighbeamColour { get { return Color.FromArgb(255, 13, 107, 229); } }

        public Brush BackgroundBrush
        {
            get
            {
                if (Type == IndicatorType.HighBeam)
                    return Value ? new SolidColorBrush(Indicator.HighbeamColour) : Helpers.Brushes.SegmentBackgroundColour;
                else
                    return Helpers.Brushes.Transparent;
            }
        }

        public Visibility HighbeamIcon
        {
            get { return Type == IndicatorType.HighBeam ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Color LeftColour
        {
            get
            {
                if (Type == IndicatorType.Left)
                    return Value ? Colors.Green : Helpers.Brushes.SegmentBackgroundColour.Color;
                else
                    return Colors.Transparent;
            }
        }

        public Color LeftGlowColour
        {
            get { return Value ? LeftColour : Colors.Transparent; }
        }

        public Brush LeftBrush
        {
            get { return new SolidColorBrush(LeftColour); }
        }

        public Color RightColour
        {
            get
            {
                if (Type == IndicatorType.Right)
                    return Value ? Colors.Green : Helpers.Brushes.SegmentBackgroundColour.Color;
                else
                    return Colors.Transparent;
            }
        }

        public Color RightGlowColour
        {
            get { return Value ? RightColour : Colors.Transparent; }
        }

        public Brush RightBrush
        {
            get { return new SolidColorBrush(RightColour); }
        }
    }
}
