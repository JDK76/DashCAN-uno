using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Shapes;

namespace DashCAN.Controls
{
    public sealed partial class StackBar : UserControl
    {
        private ViewModel.StackBar? VM { get; set; }
        private readonly Dictionary<int, Tuple<Rectangle, Grid>> Segments = new();
        private readonly List<Grid> StaticGlow = new();

        public StackBar()
        {
            this.InitializeComponent();
            this.Loaded += StackBar_Loaded;
            this.DataContextChanged += StackBar_DataContextChanged;
        }

        private void StackBar_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            VM = this.DataContext as ViewModel.StackBar;
            if (VM != null) VM.PropertyChanged += VM_PropertyChanged;
        }

        private void StackBar_Loaded(object sender, RoutedEventArgs e)
        {
            // Create glow effect for static components
            StaticGlow.Clear();
            AddGlow(topLabel, ViewModel.StackBar.LitBrush);
            AddGlow(arrow, ViewModel.StackBar.LitBrush);
            AddGlow(bottomLabel, ViewModel.StackBar.LitBrush);

            // Create glow effect for segments
            Segments.Clear();
            for (var i = 1; i <= 10; i++)
            {
                var segment = grd.FindName($"seg{i}") as Rectangle;
                if (segment != null)
                {
                    var glow = Helpers.Glow.CreateGlow(segment, ((SolidColorBrush)ViewModel.StackBar.LitBrush).Color, 5, 100, false);
                    if (glow != null) Segments.Add(i, new Tuple<Rectangle, Grid>(segment, glow));
                }
            }
        }

        private void AddGlow(FrameworkElement element, Brush brush)
        {
            var solidBrush = brush as SolidColorBrush;
            if (solidBrush != null)
            {
                var glow = Helpers.Glow.CreateGlow(element, solidBrush.Color, 5, 200);
                if (glow != null) StaticGlow.Add(glow);
            }
        }

        private void VM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value" && VM != null)
            {
                if (Segments.Count == 10)
                {
                    // Set the glow visibility for segments
                    for (var i = 1; i <= 10; i++)
                    {
                        var glow = Segments[i].Item2.Tag as SpriteVisual;
                        if (glow != null) glow.IsVisible = VM.SegmentLit(i);
                    }
                }
            }
        }
    }
}