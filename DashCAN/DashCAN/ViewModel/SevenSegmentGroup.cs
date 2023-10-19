using System.Collections.ObjectModel;

namespace DashCAN.ViewModel
{
    public class SevenSegmentGroup : ViewModelBase
    {
        public SevenSegmentGroup(int segmentCount)
        {
            SegmentCount = segmentCount;
            if (SegmentCount < 1) SegmentCount = 1;
            if (SegmentCount > 10) SegmentCount = 10;

            for (var i = 0; i < SegmentCount; i++)
            {
                Segments.Add(new SevenSegment());
            }
        }

        private readonly int SegmentCount;

        public ObservableCollection<SevenSegment> Segments { get; set; } = new();

        private int _value = 0;
        public int Value
        {
            get { return _value; }
            set { if (SetProperty(ref _value, value)) SetValue(value); }
        }

        private void SetValue(int value)
        {
            var str = value.ToString().PadLeft(SegmentCount);
            for (var i = SegmentCount - 1; i >= 0; i--)
            {
                if (Segments.Count > i)
                {
                    Segments[i].Value = str[i].ToString() == " " ? null : (int?)int.Parse(str[i].ToString());
                }
            }
        }

        public static Brush BackgroundBrush
        {
            get { return Helpers.Brushes.SegmentBackgroundColour; }
        }
    }
}
