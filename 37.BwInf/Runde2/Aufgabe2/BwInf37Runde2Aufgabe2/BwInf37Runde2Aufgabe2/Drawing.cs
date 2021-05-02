using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BwInf37Runde2Aufgabe2
{
    public class Drawing
    {
        private MainWindow mainWindow;
        private Canvas canvas;

        private const int BrickSize = 35;
        private const int CanvasGridMargin = 20;

        public Drawing(MainWindow AMainWindow)
        {
            mainWindow = AMainWindow;
            canvas = mainWindow.CanvasGrid;
        }
        public void Draw()
        {
            mainWindow.CanvasGrid.Children.Clear();
            SetCanvasScale();
            Warten(canvas);
            DrawAllBricks();
        }
        private void SetCanvasScale()
        {
            canvas.Width = Data.length * BrickSize + 2 * CanvasGridMargin;
            canvas.Height = Data.height * BrickSize + 2 * CanvasGridMargin;
        }
        private void DrawAllBricks()
        {
            DrawRectangle(new Point(0, 0), 1);
            DrawRectangle(new Point(1, 0), 2);
            DrawRectangle(new Point(3, 0), 3);
            DrawRectangle(new Point(6, 0), 4);
            DrawRectangle(new Point(10, 0), 5);
            DrawRectangle(new Point(15, 0), 6);

            DrawRectangle(new Point(1, 1), 2);
            DrawRectangle(new Point(1, 2), 2);
            DrawRectangle(new Point(1, 3), 2);
        }
        private void DrawRectangle(Point LeftDownCorner, int lenght)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Width = lenght * BrickSize;
            rectangle.Height = BrickSize;
            rectangle.Stroke = Brushes.Black;
            rectangle.StrokeThickness = 2;
            rectangle.Fill = Brushes.White;

            LeftDownCorner = PlotToCanvas(LeftDownCorner);
            Canvas.SetLeft(rectangle, LeftDownCorner.X);
            Canvas.SetTop(rectangle, LeftDownCorner.Y);

            canvas.Children.Add(rectangle);

            DrawNumber(LeftDownCorner, lenght);
        }
        private void DrawNumber(Point LeftDownCorner, int lenght)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = lenght.ToString();
            textBlock.FontSize = 25;

            Canvas.SetLeft(textBlock, LeftDownCorner.X + 10);
            Canvas.SetTop(textBlock, LeftDownCorner.Y + 0);

            canvas.Children.Add(textBlock);
        }
        private Point PlotToCanvas(Point P)
        {
            return new Point(P.X * BrickSize + CanvasGridMargin, (Data.height - P.Y - 1) * BrickSize + CanvasGridMargin);
        }

        private void Warten(Canvas canvas)
        {
            Action DummyAction = DoNothing;
            Thread.Sleep(100);
            canvas.Dispatcher.Invoke(DispatcherPriority.Input, DummyAction);
        }

        private void DoNothing() { }

    }
}
