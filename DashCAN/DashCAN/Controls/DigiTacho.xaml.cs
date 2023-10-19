using Microsoft.UI;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Shapes;
using SkiaSharp;
using Windows.UI;
using Windows.UI.Composition;

namespace DashCAN.Controls
{
    public sealed partial class DigiTacho : UserControl
    {
        private ViewModel.DigiTacho? vm;
        private readonly Color LitRed = new() { A = 255, R = 255, G = 0, B = 0 };
        private readonly Color LitOrange = new() { A = 255, R = 255, G = 186, B = 108 };
        private readonly Color LitGreen = new() { A = 255, R = 229, G = 249, B = 10 };
        private readonly Color Unlit = new() { A = 255, R = 137, G = 123, B = 120 };
        private readonly Dictionary<int, Rectangle> LEDs = new();

        public DigiTacho()
        {
            this.InitializeComponent();
            Loaded += DigiTacho_Loaded;
            DataContextChanged += DigiTacho_DataContextChanged;
        }

        private void DigiTacho_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            vm = this.DataContext as ViewModel.DigiTacho;
            if (vm != null) vm.PropertyChanged += Vm_PropertyChanged;
        }

        private void DigiTacho_Loaded(object sender, RoutedEventArgs e)
        {
            // Define the curve path
            var skPath = SKPath.ParseSvgPathData("M 2.4799634,294.20439 C 48.273472,238.79462 94.066746,183.38513 131.95361,151.64874 c 37.88686,-31.73639 67.86725,-39.79988 110.90925,-37.54842 43.042,2.25146 99.14574,14.81774 155.2493,27.38398");
            var measure = new SKPathMeasure(skPath);
            var pathLength = measure.Length;

            for (var i = 0; i <= 50; i++)
            {
                // Get the position and angle of each LED along the path
                measure.GetPositionAndTangent((pathLength / 50f) * i, out SKPoint pos, out SKPoint tangent);
                var angle = Angle(zero, tangent);

                // Create a canvas at the calculated position and angle
                var subCanvas = new Canvas()
                {
                    Margin = new Thickness(pos.X + 4, pos.Y + 4, 0, 0),
                    RenderTransform = new RotateTransform() { Angle = -angle + 180 }
                };
                canvas.Children.Add(subCanvas);

                var tick = (i % 5 == 0) ? "major" : "minor";
                if (i == 5 || i == 15) tick = "mid";

                // Add the LED background glow (rectangle with drop shadow)
                var glowRect = new Rectangle()
                {
                    Width = 12,
                    Height = 9,
                    Margin = new Thickness(-2, -1.5, 0, 0)
                };
                subCanvas.Children.Add(glowRect);
                var compositor = ElementCompositionPreview.GetElementVisual(glowRect).Compositor;
                var visual = compositor.CreateSpriteVisual();
                visual.Size = new System.Numerics.Vector2(12, 9);
#if HAS_UNO
#else
                var shadow = compositor.CreateDropShadow();
                shadow.Color = Colors.Red;
                shadow.BlurRadius = 10;
                shadow.Opacity = 0;
                visual.Shadow = shadow;
#endif
                ElementCompositionPreview.SetElementChildVisual(glowRect, visual);

                // Add the LED (coloured rectangle)
                var rect = new Rectangle()
                {
                    Width = 8,
                    Height = 6,
                    Fill = new SolidColorBrush(Unlit),
                    Tag = visual
                };
                LEDs.Add(i, rect);
                subCanvas.Children.Add(rect);

                // Add the tick mark (white line)
                double width = 1;
                if (tick == "major") width = 5; else if (tick == "mid") width = 4;
                subCanvas.Children.Add(new Line()
                {
                    StrokeThickness = width,
                    Stroke = new SolidColorBrush(Colors.White),
                    Width = 10,
                    Height = 8,
                    X1 = 5,
                    X2 = 5,
                    Y1 = 0,
                    Y2 = 8,
                    RenderTransform = new TranslateTransform() { Y = 12 }
                });

                // Add the RPM label (white single digit)
                if (tick == "major")
                {
                    var rpm = i <= 20 ? i / 10 : (i - 20) / 5 + 2;
                    canvas.Children.Add(new TextBlock()
                    {
                        Margin = CalculatePosition(pos.X - 10, pos.Y - 10, angle, 30),
                        Width = 20,
                        Height = 20,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        Foreground = new SolidColorBrush(Colors.White),
                        FontFamily = new FontFamily("Square721 Cn BT"),
                        FontSize = 18,
                        Text = rpm.ToString(),
                        RenderTransform = new CompositeTransform() { ScaleX = 1.8, TranslateX = -10 }
                    });
                }
            }
        }

        /// <summary>
        /// Calculates a position from the provided point along the provided angle and offset.
        /// </summary>
        private static Thickness CalculatePosition(double x, double y, double angle, double offset)
        {
            var radians = (angle + 180) * (Math.PI / 180);
            return new Thickness(x + (Math.Sin(radians) * offset), y + (Math.Cos(radians) * offset), 0, 0);
        }

        private void Vm_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value" && vm != null) LightLEDs(vm.Value);
        }

        /// <summary>
        /// Sets the lit/unlit state of each LED based on the provided RPM value.
        /// </summary>
        private void LightLEDs(int rpm)
        {
            for (var i = 0; i <= 50; i++)
            {
                // Determine if this LED is positioned on a major tick
                var isMajor = (i % 5 == 0);
                if (i == 5 || i == 15) isMajor = false;

                // Determine if this LED is lit up
                bool isLit;
                if (i <= 20)
                {
                    // First 2000 RPM is one LED per 100
                    isLit = rpm >= i * 100;
                }
                else
                {
                    // Remainder is one LED per 200
                    isLit = (rpm - 2000) >= (i - 20) * 200;
                }

                var colour = Unlit;
                if (isLit)
                {
                    colour = LitGreen; // default colour
                    if (i >= 40) colour = LitRed; // redline colour
                    else if (isMajor) colour = LitOrange; // major tick colour
                }

                // Set the colour of the LED and its drop shadow
                if (LEDs.TryGetValue(i, out var led))
                {
                    led.Fill = new SolidColorBrush(colour);
#if HAS_UNO
#else
                    var shadow = ((Microsoft.UI.Composition.SpriteVisual)led.Tag).Shadow as Microsoft.UI.Composition.DropShadow;
                    if (shadow != null)
                    {
                        shadow.Color = colour;
                        shadow.Opacity = isLit ? 1 : 0;
                    }
#endif
                }
            }
        }

        private readonly SKPoint zero = new (0, 0);
        const double Rad2Deg = 180.0 / Math.PI;
        private static double Angle(SKPoint start, SKPoint end)
        {
            return Math.Atan2(start.Y - end.Y, end.X - start.X) * Rad2Deg;
        }
    }
}
