namespace DashCAN.Assets.Icons
{
    public sealed partial class CheckEngine : UserControl
    {
        public CheckEngine()
        {
            this.InitializeComponent();
        }

        public Brush IconColour
        {
            get { return (Brush)GetValue(IconColourProperty); }
            set { SetValue(IconColourProperty, value); }
        }

        public static readonly DependencyProperty IconColourProperty =
            DependencyProperty.Register(nameof(IconColour), typeof(Brush), typeof(CheckEngine), new PropertyMetadata(null));
    }
}
