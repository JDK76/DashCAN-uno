namespace DashCAN.Assets.Icons
{
    public sealed partial class DoorOpen : UserControl
    {
        public DoorOpen()
        {
            this.InitializeComponent();
        }

        public Brush IconColour
        {
            get { return (Brush)GetValue(IconColourProperty); }
            set { SetValue(IconColourProperty, value); }
        }

        public static readonly DependencyProperty IconColourProperty =
            DependencyProperty.Register(nameof(IconColour), typeof(Brush), typeof(DoorOpen), new PropertyMetadata(null));
    }
}
