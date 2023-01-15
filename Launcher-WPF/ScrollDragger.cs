using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Launcher_WPF
{
    public class ScrollDragger
    {
        private readonly ScrollViewer scrollViewer;
        private readonly bool vertical;
        private readonly UIElement content;
        private Point scrollMousePoint;
        private double hOff = 1;
        private double vOff = 1;

        public ScrollDragger(UIElement content, ScrollViewer scrollViewer, bool vertical)
        {
            this.scrollViewer = scrollViewer;
            this.content = content;
            this.vertical = vertical;
            content.MouseLeftButtonDown += scrollViewer_MouseLeftButtonDown;
            content.PreviewMouseMove += scrollViewer_PreviewMouseMove;
            content.PreviewMouseLeftButtonUp += scrollViewer_PreviewMouseLeftButtonUp;
        }

        private void scrollViewer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            content.CaptureMouse();
            scrollMousePoint = e.GetPosition(scrollViewer);
            if (vertical)
            {
                hOff = scrollViewer.VerticalOffset;
            } 
            else
            {
                vOff = scrollViewer.HorizontalOffset;
            }
        }

        private void scrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (content.IsMouseCaptured)
            {
                if (vertical)
                {
                    var newOffset = hOff + (scrollMousePoint.Y - e.GetPosition(scrollViewer).Y);
                    scrollViewer.ScrollToVerticalOffset(newOffset);
                }
                else
                {
                    var newOffset = vOff + (scrollMousePoint.X - e.GetPosition(scrollViewer).X);
                    scrollViewer.ScrollToHorizontalOffset(newOffset);
                }
            }
        }

        private void scrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            content.ReleaseMouseCapture();
        }
    }
}
