﻿using Microsoft.UI.Composition;

namespace DashCAN.Controls
{
    public sealed partial class SevenSegment : UserControl
    {
        private ViewModel.SevenSegment? VM { get; set; }

        public SevenSegment()
        {
            this.InitializeComponent();
            this.DataContextChanged += this.SevenSegment_DataContextChanged;
        }

        private void SevenSegment_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            VM = this.DataContext as ViewModel.SevenSegment;
            if (VM == null) return;

            VM.PropertyChanged += VM_PropertyChanged;

            // Create glow effect for segments
            for (var i = 1; i <= 7; i++)
            {
                if (grd.FindName($"seg{i}glow") is Grid segmentGlow)
                {
                    Helpers.Glow.AddGlow(segmentGlow, Helpers.Brushes.SegmentLit.Color, 1, 100, false);
                }
            }
        }

        private void VM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value" && VM != null)
            {
                // Set the glow visibility for segments
                for (var i = 1; i <= 7; i++)
                {
                    var segment = grd.FindName($"seg{i}glow") as Grid;
                    if (segment?.Tag is SpriteVisual glow) glow.IsVisible = VM.IsLit(i);
                }
            }
        }
    }
}