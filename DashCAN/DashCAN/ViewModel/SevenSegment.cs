namespace DashCAN.ViewModel
{
    public class SevenSegment : ViewModelBase
    {
        private int? _value = int.MaxValue;
        public int? Value
        {
            get { return _value; }
            set
            {
                if (SetProperty(ref _value, value)) SetSegments();
            }
        }

        private Brush? _seg1Fill;
        public Brush? Seg1Fill
        {
            get { return _seg1Fill; }
            set { SetProperty(ref _seg1Fill, value); }
        }

        private Brush? _seg2Fill;
        public Brush? Seg2Fill
        {
            get { return _seg2Fill; }
            set { SetProperty(ref _seg2Fill, value); }
        }

        private Brush? _seg3Fill;
        public Brush? Seg3Fill
        {
            get { return _seg3Fill; }
            set { SetProperty(ref _seg3Fill, value); }
        }

        private Brush? _seg4Fill;
        public Brush? Seg4Fill
        {
            get { return _seg4Fill; }
            set { SetProperty(ref _seg4Fill, value); }
        }

        private Brush? _seg5Fill;
        public Brush? Seg5Fill
        {
            get { return _seg5Fill; }
            set { SetProperty(ref _seg5Fill, value); }
        }

        private Brush? _seg6Fill;
        public Brush? Seg6Fill
        {
            get { return _seg6Fill; }
            set { SetProperty(ref _seg6Fill, value); }
        }

        private Brush? _seg7Fill;
        public Brush? Seg7Fill
        {
            get { return _seg7Fill; }
            set { SetProperty(ref _seg7Fill, value); }
        }

        /// <summary>
        /// Sets the fill property for each of the segments based on the specified value.
        /// </summary>
        private void SetSegments()
        {
            for (int segNo = 1; segNo <= 7; segNo++)
            {
                var prop = this.GetType().GetProperty($"Seg{segNo}Fill");
                if (prop != null)
                {
                    if (IsLit(segNo))
                        prop.SetValue(this, Helpers.Brushes.SegmentLit);
                    else
                        prop.SetValue(this, Helpers.Brushes.SegmentUnlit);
                }
            }
        }

        /// <summary>
        /// Determines if the specified segment should be lit based on the current value.
        /// </summary>
        public bool IsLit(int segNo)
        {
            var segments = Array.Empty<int>();
            if (Value.HasValue)
            {
                var value = Value.Value;
                if (value < 0 || value > 9) value = 0;
                switch (value)
                {
                    case 0:
                        segments = new int[] { 1, 2, 3, 4, 5, 6 };
                        break;
                    case 1:
                        segments = new int[] { 3, 4 };
                        break;
                    case 2:
                        segments = new int[] { 2, 3, 5, 6, 7 };
                        break;
                    case 3:
                        segments = new int[] { 2, 3, 4, 5, 7 };
                        break;
                    case 4:
                        segments = new int[] { 1, 3, 4, 7 };
                        break;
                    case 5:
                        segments = new int[] { 1, 2, 4, 5, 7 };
                        break;
                    case 6:
                        segments = new int[] { 1, 2, 4, 5, 6, 7 };
                        break;
                    case 7:
                        segments = new int[] { 2, 3, 4 };
                        break;
                    case 8:
                        segments = new int[] { 1, 2, 3, 4, 5, 6, 7 };
                        break;
                    case 9:
                        segments = new int[] { 1, 2, 3, 4, 5, 7 };
                        break;
                }
            }

            return segments.Contains(segNo);
        }
    }
}
