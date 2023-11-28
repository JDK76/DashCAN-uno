namespace DashCAN.ViewModel
{
    public class Dial : ViewModelBase
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        private double _value;
        public double Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
    }
}
