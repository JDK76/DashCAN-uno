using Microsoft.UI;
using Windows.UI;

namespace DashCAN.Helpers
{
    public class Brushes
    {
        public static readonly SolidColorBrush SegmentLitColour = new(Color.FromArgb(255, 56, 193, 150));
        public static readonly SolidColorBrush SegmentUnlitColour = new(Color.FromArgb(255, 20, 29, 30));
        public static readonly SolidColorBrush SegmentBackgroundColour = new(Color.FromArgb(255, 16, 25, 26));
        public static readonly SolidColorBrush Transparent = new(Colors.Transparent);
    }
}