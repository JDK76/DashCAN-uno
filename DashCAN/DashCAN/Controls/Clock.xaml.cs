namespace DashCAN.Controls
{
    public sealed partial class Clock : UserControl
    {
        public Clock()
        {
            this.InitializeComponent();
            var timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();
        }

        public ViewModel.SevenSegment Segment1 { get; set; } = new();
        public ViewModel.SevenSegment Segment2 { get; set; } = new();
        public ViewModel.SevenSegment Segment3 { get; set; } = new();
        public ViewModel.SevenSegment Segment4 { get; set; } = new();

        public Brush SeparatorFill => Helpers.Brushes.SegmentLit;

        private void Timer_Tick(object? sender, object e)
        {
            if (DateTime.Now.Second % 2 == 0)
            {
                tick1.Fill = tick2.Fill = Helpers.Brushes.SegmentLit;
            }
            else
            {
                tick1.Fill = tick2.Fill = Helpers.Brushes.SegmentUnlit;
            }

            SetTime(DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00"));
        }

        private string LastTimeString { get; set; }

        private void SetTime(string timeStr)
        {
            if (timeStr == LastTimeString) return;
            LastTimeString = timeStr;
            Segment1.Value = int.Parse(timeStr[0].ToString());
            Segment2.Value = int.Parse(timeStr[1].ToString());
            Segment3.Value = int.Parse(timeStr[2].ToString());
            Segment4.Value = int.Parse(timeStr[3].ToString());
        }
    }
}
