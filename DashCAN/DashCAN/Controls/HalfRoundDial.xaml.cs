using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.UI;

namespace DashCAN.Controls
{
    public sealed partial class HalfRoundDial : UserControl
    {
        private ViewModel.Dial? VM { get; set; }

        public HalfRoundDial()
        {
            this.InitializeComponent();
            this.Loaded += HalfRoundDial_Loaded;
            this.DataContextChanged += HalfRoundDial_DataContextChanged;
        }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int BigDivision { get; set; }
        public int MediumDivision { get; set; }
        public int SmallDivision { get; set; }
        public bool ReverseLayout { get; set; }
        public int LabelMultiplier { get; set; } = 1;

        private int ZeroAngle { get { return ReverseLayout ? -200 : 180; } }
        private Point DialCentrePoint { get; set; }
        private RotateTransform DialPointerTransform { get; set; } = new();

        private void HalfRoundDial_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            VM = this.DataContext as ViewModel.Dial;
            if (VM != null)
            {
                VM.PropertyChanged -= VM_PropertyChanged;
                VM.PropertyChanged += VM_PropertyChanged;
            }
        }

        private void VM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (VM != null) DialPointerTransform.Angle = (double)DegreesFromValue(VM.Value) - (ReverseLayout ? 20 : 0);
        }

        private void HalfRoundDial_Loaded(object sender, RoutedEventArgs e)
        {
            DialCentrePoint = new Point(canvas.Width / 2, canvas.Height * 0.7);
            DialPointerTransform.CenterX = DialCentrePoint.X;
            DialPointerTransform.CenterY = DialCentrePoint.Y;

            var outerRad = canvas.Width / 2.05;
            for (var i = MinValue; i <= MaxValue; i++)
            {
                var thickness = 25d;
                var innerRad = outerRad * 0.95;
                if (i % BigDivision == 0)
                {
                    innerRad = outerRad * 0.9;
                }
                else if (i % MediumDivision == 0)
                {
                    thickness = 15;
                }
                else if (i % SmallDivision == 0)
                {
                    thickness = 10;
                }
                else
                {
                    continue;
                }

                var degrees = DegreesFromValue(i);

                // Tick marks
                var p1 = GetRadiusPoint(DialCentrePoint, outerRad, degrees);
                var p2 = GetRadiusPoint(DialCentrePoint, innerRad, degrees);
                canvas.Children.Add(new Line()
                {
                    StrokeThickness = thickness,
                    Stroke = new SolidColorBrush(Colors.White),
                    X1 = p1.X,
                    X2 = p2.X,
                    Y1 = p1.Y,
                    Y2 = p2.Y
                });

                // Labels
                if (i % BigDivision == 0)
                {
                    var txt = new TextBlock()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        Foreground = new SolidColorBrush(Colors.White),
                        //FontFamily = new FontFamily("ms-appx:///DashCAN/Assets/Fonts/square_721_condensed_bt.ttf#Square721 Cn BT"),
                        FontFamily = new FontFamily("ms-appx:///DashCAN/Assets/Fonts/square_721_bold.otf#Square 721"),
                        FontSize = 80,
                        //FontWeight = FontWeights.ExtraBold,
                        Text = (i / LabelMultiplier).ToString()
                    };
                    txt.Measure(new Size(0, 0));
                    var point = GetRadiusPoint(DialCentrePoint, outerRad * 0.9 - txt.ActualWidth * 0.6, degrees);
                    //canvas.Children.Add(new Ellipse()
                    //{
                    //    Fill = new SolidColorBrush(Colors.Red),
                    //    Width = 4,
                    //    Height = 4,
                    //    Margin = new Thickness(point.X - 2, point.Y - 2, 0, 0)
                    //});
                    txt.Margin = new Thickness(point.X - txt.ActualWidth / 2, point.Y - txt.ActualHeight / 2, 0, 0);
                    canvas.Children.Add(txt);
                }
            }

            // Pointer
            var zeroPoint = new Point(DialCentrePoint.X - ((double)outerRad * 0.95), DialCentrePoint.Y);
            canvas.Children.Add(new Polygon()
            {
                Fill = new SolidColorBrush(Colors.Orange),
                Points = { new Point(DialCentrePoint.X, DialCentrePoint.Y + 20), zeroPoint, new Point(DialCentrePoint.X, DialCentrePoint.Y - 20) },
                RenderTransform = DialPointerTransform
            });
        }

        private double DegreesFromValue(decimal value)
        {
            return (double)(value / MaxValue * 200);
        }

        private Point GetRadiusPoint(Point centre, double radius, double degrees)
        {
            var radians = (Math.PI / 180) * (ZeroAngle + degrees);
            return new Point(centre.X + radius * Math.Cos(radians), centre.Y + radius * Math.Sin(radians));
        }
    }
}
