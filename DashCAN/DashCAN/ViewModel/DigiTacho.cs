namespace DashCAN.ViewModel
{
    public class DigiTacho : ViewModelBase
    {
        private int _value;
        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
    }
}