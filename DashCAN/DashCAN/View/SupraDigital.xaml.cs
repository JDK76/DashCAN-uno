using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace DashCAN.View
{
    public sealed partial class SupraDigital : Page
    {
        private readonly float TileSize = 110;
        private readonly float XOffset = 80;
        private readonly float YOffset = 85;

        public SupraDigital()
        {
            this.InitializeComponent();
            this.Loaded += SupraDigital_Loaded;
            this.SizeChanged += SupraDigital_SizeChanged;
            bkg.Background = new SolidColorBrush(Color.FromArgb(255, 31, 58, 49));
        }

        private void SupraDigital_Loaded(object sender, RoutedEventArgs e)
        {
            // Create glow around speed units text
            var brush = (speedText.Children[0] as TextBlock)?.Foreground as SolidColorBrush;
            if (brush != null)
            {
                var colour = brush.Color;
                colour.A = 127;
                Helpers.Glow.AddGlow(speedText, colour, 1, 150, true);
            }
        }

        private void SupraDigital_SizeChanged(object sender, SizeChangedEventArgs args)
        {
            DrawPattern();
        }

        private void DrawPattern()
        {
            float width, height;
            width = height = TileSize;
            var lineWidth = TileSize / 25.0f;

            for (var x = XOffset; x < bkg.ActualWidth; x += width)
            {
                bkg.Children.Add(new Line()
                {
                    StrokeThickness = lineWidth,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 16, 25, 26)),
                    Width = bkg.ActualWidth,
                    Height = bkg.ActualHeight,
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = bkg.ActualHeight
                });
            }

            for (var y = YOffset; y < bkg.ActualHeight; y += height)
            {
                bkg.Children.Add(new Line()
                {
                    StrokeThickness = lineWidth,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 16, 25, 26)),
                    Width = bkg.ActualWidth,
                    Height = bkg.ActualHeight,
                    X1 = 0,
                    Y1 = y,
                    X2 = bkg.ActualWidth,
                    Y2 = y
                });
            }
        }
    }
}
