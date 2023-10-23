using Microsoft.UI;
using Microsoft.UI.Composition;

namespace DashCAN.Controls
{
    public sealed partial class HighBeam : UserControl
    {
        private Grid? BackGlow { get; set; }
        private ViewModel.Indicator? VM { get; set; }

        public HighBeam()
        {
            this.InitializeComponent();
            this.DataContextChanged += HighBeam_DataContextChanged;
        }

        private void HighBeam_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            VM = this.DataContext as ViewModel.Indicator;
            if (VM != null)
            {
                VM.PropertyChanged += VM_PropertyChanged;
                BackGlow = Helpers.Glow.CreateGlow(glow, Colors.Transparent, 12.5f, 100);
            }
        }

        private void VM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
#if HAS_UNO
#else
            if (e.PropertyName == "Value" && VM != null)
            {
                var shadow = (BackGlow?.Tag as SpriteVisual)?.Shadow as DropShadow;
                if (shadow != null)
                {
                    var backBrush = (VM.HighBeamBrush as SolidColorBrush) ?? new SolidColorBrush(Colors.Transparent);
                    shadow.Color = VM.Value ? backBrush.Color : Colors.Transparent;
                }
            }
#endif
        }
    }
}
