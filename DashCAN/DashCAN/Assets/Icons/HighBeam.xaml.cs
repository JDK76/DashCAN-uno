namespace DashCAN.Assets.Icons
{
    public sealed partial class HighBeam : UserControl
    {
        public HighBeam()
        {
            this.InitializeComponent();
        }

        public Brush IconColour
        {
            get { return (Brush)GetValue(IconColourProperty); }
            set { SetValue(IconColourProperty, value); }
        }

        public static readonly DependencyProperty IconColourProperty =
            DependencyProperty.Register(nameof(IconColour), typeof(Brush), typeof(HighBeam), new PropertyMetadata(null));
    }
}
