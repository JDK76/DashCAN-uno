namespace DashCAN.Helpers
{
    /*
    public class TilingBrush : XamlCompositionBrushBase
    {
        protected Compositor _compositor => Window.Current.Compositor;

        protected CompositionBrush? _imageBrush = null;

        protected IDisposable? _surfaceSource = null;

        public float? TileSize { get; set; }
        public float? XOffset { get; set; }
        public float? YOffset { get; set; }

        protected override void OnConnected()
        {
            base.OnConnected();

            if (CompositionBrush == null)
            {
                CreateEffectBrush();
                Render();
            }
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            this.CompositionBrush?.Dispose();
            this.CompositionBrush = null;

            ClearResources();
        }

        private void ClearResources()
        {
            _imageBrush?.Dispose();
            _imageBrush = null;

            _surfaceSource?.Dispose();
            _surfaceSource = null;
        }

        private void UpdateBrush()
        {
            if (CompositionBrush != null && _imageBrush != null)
            {
                ((CompositionEffectBrush)CompositionBrush).SetSourceParameter(nameof(BorderEffect.Source), _imageBrush);
            }
        }

        protected ICompositionSurface CreateSurface()
        {
            float width = 100;
            float height = 100;
            float lineWidth = 5;
            if (TileSize.HasValue)
            {
                width = height = TileSize.Value;
                lineWidth = TileSize.Value / 25.0f;
            }

            CanvasDevice device = CanvasDevice.GetSharedDevice();
            var graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(_compositor, device);
            var drawingSurface = graphicsDevice.CreateDrawingSurface(
                new Size(width, height),
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DirectXAlphaMode.Premultiplied);

            // Create Drawing Session is not thread safe - only one can ever be active at a time per app 
            using (var ds = CanvasComposition.CreateDrawingSession(drawingSurface))
            {
                ds.Clear(Windows.UI.Color.FromArgb(255, 31, 58, 49));
                ds.DrawLine(new Vector2(width / 2.0f, 0), new Vector2(width / 2.0f, width), Color.FromArgb(255, 16, 25, 26), lineWidth);
                ds.DrawLine(new Vector2(0, (YOffset ?? 0f) + width / 2.0f), new Vector2(width, (YOffset ?? 0f) + width / 2.0f), Color.FromArgb(255, 16, 25, 26), lineWidth);
            }

            return drawingSurface;
        }

        private void Render()
        {
            ClearResources();

            try
            {
                var src = CreateSurface();
                _surfaceSource = src as IDisposable;
                var surfaceBrush = _compositor.CreateSurfaceBrush(src);
                surfaceBrush.VerticalAlignmentRatio = 0.0f;
                surfaceBrush.HorizontalAlignmentRatio = 0.0f;
                surfaceBrush.Stretch = CompositionStretch.None;
                _imageBrush = surfaceBrush;

                UpdateBrush();
            }
            catch
            {
                // no image for you, soz.
            }
        }

        private void CreateEffectBrush()
        {
            using (var effect = new BorderEffect
            {
                Name = nameof(BorderEffect),
                ExtendY = CanvasEdgeBehavior.Wrap,
                ExtendX = CanvasEdgeBehavior.Wrap,
                Source = new CompositionEffectSourceParameter(nameof(BorderEffect.Source))
            })
            using (var _effectFactory = _compositor.CreateEffectFactory(effect))
            {
                this.CompositionBrush = _effectFactory.CreateBrush();
            }
        }
    }
    */
}
