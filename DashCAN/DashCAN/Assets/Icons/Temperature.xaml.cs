namespace DashCAN.Assets.Icons
{
    public sealed partial class Temperature : UserControl
    {
        public Temperature()
        {
            this.InitializeComponent();
        }

        public Brush IconColour
        {
            get { return (Brush)GetValue(IconColourProperty); }
            set { SetValue(IconColourProperty, value); }
        }

        public static readonly DependencyProperty IconColourProperty =
            DependencyProperty.Register(nameof(IconColour), typeof(Brush), typeof(Fuel), new PropertyMetadata(null));
    }
}
