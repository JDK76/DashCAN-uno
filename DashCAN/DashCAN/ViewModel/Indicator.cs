using DashCAN.Common;

namespace DashCAN.ViewModel
{
    public class Indicator : InstrumentValue
    {
        public Indicator(IndicatorType type, DataValue dataValue) : base(Unit.Boolean, dataValue)
        {
            Type = type;
        }

        private IndicatorType _type = IndicatorType.None;
        public IndicatorType Type
        {
            get { return _type; }
            private set { SetProperty(ref _type, value); }
        }

        private bool _value;
        public bool Value
        {
            get { return _value; }
            private set { if (SetProperty(ref _value, value)) OnPropertyChanged(new string[] { nameof(HighBeamBrush), nameof(LeftBrush), nameof(RightBrush) }); }
        }

        protected override void SetValue(DataValue value)
        {
            var boolValue = (value as BoolValue)?.IsSet;
            if (boolValue.HasValue) Value = boolValue.Value;
        }

        public Brush HighBeamBrush { get { return Value ? Helpers.Brushes.HighBeam : Helpers.Brushes.SegmentBackground; } }

        public Brush LeftBrush
        {
            get
            {
                if (Type != IndicatorType.Left) return Helpers.Brushes.Transparent;
                return Value ? IndicatorBrush : Helpers.Brushes.SegmentBackground;
            }
        }

        public Brush RightBrush
        {
            get
            {
                if (Type != IndicatorType.Right) return Helpers.Brushes.Transparent;
                return Value ? IndicatorBrush : Helpers.Brushes.SegmentBackground;
            }
        }

        public Brush IndicatorBrush => Helpers.Brushes.Indicator;
    }
}
