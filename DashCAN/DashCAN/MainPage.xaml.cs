using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace DashCAN
{
    public sealed partial class MainPage : Page
    {
        private readonly float TileSize = 110;
        private readonly float XOffset = 80;
        private readonly float YOffset = 85;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            SizeChanged += MainPage_SizeChanged;
            bkg.Background = new SolidColorBrush(Color.FromArgb(255, 31, 58, 49));
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModel.Main();
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
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