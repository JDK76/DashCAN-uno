using Microsoft.UI.Xaml.Hosting;
using System.Numerics;
using Windows.UI;

namespace DashCAN.Helpers
{
    public class Glow
    {
        /// <summary>
        /// Creates a glow effect around the provided base element.
        /// The base element must be contained in a grid element.
        /// </summary>
        public static Grid? CreateGlow(FrameworkElement baseElement, Color glowColour, float glowMargin, float blurRadius, bool visible = true)
        {
            // Base element must be contained in a grid element, otherwise abort
            if (baseElement.Parent is not Grid parent) return null;
            return CreateGlow(baseElement, parent, glowColour, glowMargin, blurRadius, visible);
        }

        public static Grid? CreateGlow(FrameworkElement baseElement, Grid parent, Color glowColour, float glowMargin, float blurRadius, bool visible = true)
        { 
            // Create a new grid container with the same size and alignment as the base element
            var glow = new Grid()
            {
                Width = baseElement.ActualWidth,
                Height = baseElement.ActualHeight,
                Margin = baseElement.Margin,
                HorizontalAlignment = baseElement.HorizontalAlignment,
                VerticalAlignment = baseElement.VerticalAlignment
            };
            glow.SetValue(Grid.RowProperty, baseElement.GetValue(Grid.RowProperty));
            glow.SetValue(Grid.RowSpanProperty, baseElement.GetValue(Grid.RowSpanProperty));
            glow.SetValue(Grid.ColumnProperty, baseElement.GetValue(Grid.ColumnProperty));
            glow.SetValue(Grid.ColumnSpanProperty, baseElement.GetValue(Grid.ColumnSpanProperty));
            parent.Children.Add(glow);

            // Create a new sprite visial over the grid container
            var compositor = ElementCompositionPreview.GetElementVisual(glow).Compositor;
            var basicRectVisual = compositor.CreateSpriteVisual();
            basicRectVisual.Size = new Vector2((float)glow.Width + glowMargin * 2, (float)glow.Height + glowMargin * 2);
            basicRectVisual.Offset = new Vector3(-glowMargin, -glowMargin, 0f);
            basicRectVisual.IsVisible = visible;
            ElementCompositionPreview.SetElementChildVisual(glow, basicRectVisual);

#if HAS_UNO
#else
            // Create the drop shadow
            var shadow = compositor.CreateDropShadow();
            shadow.BlurRadius = blurRadius;
            shadow.Color = glowColour;
            basicRectVisual.Shadow = shadow;
#endif

            // Return the grid container with the sprite visual as its tag
            glow.Tag = basicRectVisual;
            return glow;
        }

        public static void AddGlow(FrameworkElement element, Color glowColour, float glowMargin, float blurRadius, bool visible = true)
        {
#if HAS_UNO
#else
            // Create a new sprite visial over the element
            var compositor = ElementCompositionPreview.GetElementVisual(element).Compositor;
            var basicRectVisual = compositor.CreateSpriteVisual();
            basicRectVisual.Size = new Vector2((float)element.Width + glowMargin * 2, (float)element.Height + glowMargin * 2);
            basicRectVisual.Offset = new Vector3(-glowMargin, -glowMargin, 0f);
            basicRectVisual.IsVisible = visible;
            ElementCompositionPreview.SetElementChildVisual(element, basicRectVisual);

            // Create the drop shadow
            var shadow = compositor.CreateDropShadow();
            shadow.BlurRadius = blurRadius;
            shadow.Color = glowColour;
            basicRectVisual.Shadow = shadow;

            // Return the element with the sprite visual as its tag
            element.Tag = basicRectVisual;
#endif
        }
    }
}