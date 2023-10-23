using Microsoft.UI;
using Windows.UI;

namespace DashCAN.Helpers
{
    public class Brushes
    {
        public static readonly SolidColorBrush HighBeam = new(Color.FromArgb(255, 13, 107, 229));
        public static readonly SolidColorBrush Indicator = new(Color.FromArgb(255, 0, 255, 0));
        public static readonly SolidColorBrush SegmentBackground = new(Color.FromArgb(255, 16, 25, 26));
        public static readonly SolidColorBrush SegmentLit = new(Color.FromArgb(255, 56, 193, 150));
        public static readonly SolidColorBrush SegmentUnlit = new(Color.FromArgb(255, 20, 29, 30));
        public static readonly SolidColorBrush Transparent = new(Colors.Transparent);
        public static readonly SolidColorBrush WarningLitOrange = new(Color.FromArgb(255, 255, 127, 0));
        public static readonly SolidColorBrush WarningLitRed = new(Color.FromArgb(255, 255, 0, 0));
        public static readonly SolidColorBrush WarningUnlit = new(Color.FromArgb(255, 25, 25, 25));
    }
}