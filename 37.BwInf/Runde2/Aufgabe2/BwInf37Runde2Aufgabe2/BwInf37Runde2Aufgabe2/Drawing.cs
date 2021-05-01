using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BwInf37Runde2Aufgabe2
{
    public class Drawing
    {
        private MainWindow mainWindow;
        private Canvas canvas;

        private const int CanvasGridMargin = 20;
        private double DX, DY;
        private double X0, Y0;
        private double MinX, MaxX, MinY, MaxY;

        public Drawing(MainWindow AMainWindow)
        {
            mainWindow = AMainWindow;
            canvas = mainWindow.CanvasGrid;

        }
        public void Draw()
        {
            mainWindow.CanvasGrid.Children.Clear();
            SetParameters();
            SetScale();
            DrawAllBricks();
        }
        private void SetParameters()
        {
            MinX = 0;
            MaxX = Data.length;
            MinY = 0;
            MaxY = Data.height;
        }
        private void SetScale()
        {
            //Diese Methode berechnet Werte, die für die Umrechnung des mathematischen Koordinatensystems in das Koordinatensystems des Canvas benötigt werden
            DX = (canvas.ActualWidth - 2 * CanvasGridMargin) / (MaxX - MinX);
            X0 = CanvasGridMargin - DX * MinX;
            DY = (canvas.ActualHeight - 2 * CanvasGridMargin) / (MinY - MaxY);
            Y0 = CanvasGridMargin - DY * MaxY;
        }
        private void DrawAllBricks()
        {
            DrawRectangle(new Point(0, 0), 1);
            DrawRectangle(new Point(1, 0), 2);
            DrawRectangle(new Point(3, 0), 3);
            DrawRectangle(new Point(6, 0), 4);

            DrawRectangle(new Point(1, 1), 2);
            DrawRectangle(new Point(1, 2), 2);
            DrawRectangle(new Point(1, 3), 2);
            DrawRectangle(new Point(1, 4), 2);
            DrawRectangle(new Point(1, 5), 2);
        }
        private void DrawRectangle(Point LeftDownCorner, int lenght)
        {
            LeftDownCorner = PlotToCanvas(LeftDownCorner);

            Rectangle rectangle = new Rectangle();
            rectangle.Width = lenght * DX;
            rectangle.Height = -DY;

            Canvas.SetLeft(rectangle, LeftDownCorner.X);
            Canvas.SetTop(rectangle, LeftDownCorner.Y + DY);
            rectangle.Stroke = Brushes.Black;
            rectangle.StrokeThickness = 2;
            rectangle.Fill = Brushes.White;

            canvas.Children.Add(rectangle);
        }
        
        private Point PlotToCanvas(Point P)
        {
            //Diese Methode rechnet die mathematischen Koordinaten in die Koordinaten auf dem Canvas um
            return new Point(DX * P.X + X0, DY * P.Y + Y0);
        }

    }
}
