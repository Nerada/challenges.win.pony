//-----------------------------------------------
//      Author: Ramon Bollen
//       File: Challenge1.Support.LimitChild.cs
// Created on: 2019105
//-----------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace Challenge1.Support
{
    /// <summary>
    /// https://stackoverflow.com/questions/21255551/wpf-how-to-make-a-textbox-dynamically-sized-yet-prevent-autosizing
    /// </summary>
    internal class LimitChild : Panel
    {
        public LimitChild()
        {
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            System.Diagnostics.Debug.Assert(InternalChildren.Count == 1);
            UIElement child = InternalChildren[0];

            var panelDesiredSize = new Size()
            {
                // panelDesiredSize.Width = availableSize.Width;
                Width = (double)child.GetValue(FrameworkElement.MinWidthProperty),
                Height = (double)child.GetValue(FrameworkElement.MinHeightProperty)
            };

            child.Measure(panelDesiredSize);

            // IMPORTANT: do not allow PositiveInfinity to be returned, that will raise an exception in the caller!
            // PositiveInfinity might be an availableSize input; this means that the parent does not care about sizing
            return panelDesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            UIElement child = InternalChildren[0];

            child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            if (finalSize.Width > child.RenderSize.Width)
            {
                finalSize.Width = child.RenderSize.Width;
            }

            if (finalSize.Height > child.RenderSize.Height)
            {
                finalSize.Height = child.RenderSize.Height;
            }

            return finalSize; // Returns the final Arranged size
        }
    }
}