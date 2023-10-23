using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace DashCAN.ViewModel
{
    public class StackBar : ViewModelBase
    {
        private readonly bool Stacked;
        private readonly bool FlashOnMax;
        private readonly double MinValue;
        private readonly double MaxValue;
        private readonly System.Timers.Timer Flasher;
        private bool FlasherOn;

        public StackBar(bool stacked, bool flashOnMax, string minLabel, string maxLabel, double minValue, double maxValue, double? value = null)
        {
            Flasher = new System.Timers.Timer(200);
            Flasher.Elapsed += Flasher_Elapsed;

            Stacked = stacked;
            FlashOnMax = flashOnMax;
            MinLabel = minLabel;
            MaxLabel = maxLabel;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value ?? 0;
        }

        private void Flasher_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Flasher.Interval = GlobalUnlit ? 400 : 200;
                _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => GlobalUnlit = !GlobalUnlit);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static Brush BackgroundBrush
        {
            get { return Helpers.Brushes.SegmentBackground; }
        }

        public static Brush LitBrush
        {
            get { return Helpers.Brushes.SegmentLit; }
        }

        private bool _globalUnlit;
        public bool GlobalUnlit
        {
            get { return _globalUnlit; }
            private set
            {
                if (SetProperty(ref _globalUnlit, value))
                {
                    OnPropertyChanged(Enumerable.Range(1, 10).Select(i => $"Seg{i}Fill").Concat(new string[] { "Value" }));
                }
            }
        }

        public string MinLabel { get; set; }

        public string MaxLabel { get; set; }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                var newVal = value;
                if (newVal < MinValue) newVal = MinValue;
                if (newVal > MaxValue)
                {
                    newVal = MaxValue;
                    if (FlashOnMax && !FlasherOn)
                    {
                        // Max value exceeded, start the flasher
                        GlobalUnlit = true;
                        Flasher.Interval = 200;
                        Flasher.Start();
                        FlasherOn = true;
                    }
                }
                else
                {
                    GlobalUnlit = false;
                    if (FlasherOn)
                    {
                        // Max value no longer exceeded, stop the flasher
                        Flasher.Stop();
                        FlasherOn = false;
                    }
                }

                if (SetProperty(ref _value, newVal))
                {
                    OnPropertyChanged(Enumerable.Range(1, 10).Select(i => $"Seg{i}Fill"));
                }
            }
        }

        public Brush Seg1Fill
        {
            get
            {
                return SegmentLit(1) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg2Fill
        {
            get
            {
                return SegmentLit(2) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg3Fill
        {
            get
            {
                return SegmentLit(3) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg4Fill
        {
            get
            {
                return SegmentLit(4) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg5Fill
        {
            get
            {
                return SegmentLit(5) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg6Fill
        {
            get
            {
                return SegmentLit(6) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg7Fill
        {
            get
            {
                return SegmentLit(7) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg8Fill
        {
            get
            {
                return SegmentLit(8) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg9Fill
        {
            get
            {
                return SegmentLit(9) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        public Brush Seg10Fill
        {
            get
            {
                return SegmentLit(10) ? Helpers.Brushes.SegmentLit : Helpers.Brushes.SegmentUnlit;
            }
        }

        private double FullRange { get { return MaxValue - MinValue; } }

        private double SegmentRange { get { return FullRange / 10.0; } }

        /// <summary>
        /// Determines wherther the specified segment should be lit up.
        /// </summary>
        public bool SegmentLit(int segNo)
        {
            if (!GlobalUnlit && Value >= (MinValue + (SegmentRange * (segNo - 1))))
            {
                if (Stacked)
                {
                    return true;
                }
                else
                {
                    if (Value <= (SegmentRange * segNo)) return true;
                }
            }

            return false;
        }
    }
}
