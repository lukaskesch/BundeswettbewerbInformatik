﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BwInf36Runde2Aufgabe1
{
    public class Drawing
    {
        private Data data;
        private MainWindow mainWindow;
        private Canvas canvas;

        private const int BrickSize = 35;
        private const int CanvasGridMargin = 20;

        public Drawing(MainWindow AMainWindow, Data AData)
        {
            mainWindow = AMainWindow;
            canvas = mainWindow.CanvasGrid;
            data = AData;
        }
        public void Draw()
        {
            LockUI();

            if (!data.OddNumberOfBricks)
            {
                data.OddNumberOfBricks = true;
                SetCanvasScale();
                Wait(canvas);
                DrawAllBricks();
                Wait(canvas);
                SaveImage1();
                data.OddNumberOfBricks = false;
            }

            mainWindow.CanvasGrid.Children.Clear();
            SetCanvasScale();
            Wait(canvas);
            DrawAllBricks();
            Wait(canvas);
            SaveImage1();
            //SaveImage2();
            UnlockUI();
        }
        private void LockUI()
        {
            canvas.LayoutTransform = null;
            mainWindow.CanvasGrid.Children.Clear();
            mainWindow.ResizeMode = ResizeMode.NoResize;
            mainWindow.WindowState = WindowState.Minimized;
            //Thread.SpinWait(500);
        }
        private void UnlockUI()
        {
            mainWindow.ResizeMode = ResizeMode.CanResize;
            mainWindow.WindowState = WindowState.Normal;
        }
        private void SetCanvasScale()
        {
            if (data.OddNumberOfBricks)
                canvas.Width = (data.length + data.NumberOfBricks + 1) * BrickSize + 2 * CanvasGridMargin;
            else
                canvas.Width = data.length * BrickSize + 2 * CanvasGridMargin;

            canvas.Height = data.height * BrickSize + 2 * CanvasGridMargin;
        }
        private void DrawAllBricks()
        {
            int BrickLength, SlotNumber;
            for (int i = 0; i < data.height; i++)
            {
                SlotNumber = 0;
                for (int j = 0; j < data.NumberOfBricks; j++)
                {
                    if (j == 2 && data.OddNumberOfBricks)
                    {
                        BrickLength = data.NumberOfBricks + 1;
                        DrawRectangle(new Point(SlotNumber, i), BrickLength);
                        SlotNumber += BrickLength;
                    }

                    BrickLength = data.Bricks[i, j];
                    DrawRectangle(new Point(SlotNumber, i), BrickLength);
                    SlotNumber += BrickLength;
                }
            }
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
            return new Point(P.X * BrickSize + CanvasGridMargin, (data.height - P.Y - 1) * BrickSize + CanvasGridMargin);
        }

        private void Wait(Canvas canvas)
        {
            Action DummyAction = DoNothing;
            Thread.Sleep(10);
            canvas.Dispatcher.Invoke(DispatcherPriority.Input, DummyAction);
        }

        private void DoNothing() { }

        private void SaveImage() //Image is too compressed
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();


                Directory.CreateDirectory("Results");
                string path = string.Format(@"Results\{0}-0.png", data.NumberOfBricks.ToString());
                System.IO.File.WriteAllBytes(path, ms.ToArray());
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveImage1()
        {

            // Save current canvas transform
            Transform transform = canvas.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            canvas.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(canvas.Width, canvas.Height);
            // Measure and arrange the surface
            // VERY IMPORTANT
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(canvas);

            // Create a file stream for saving image
            Directory.CreateDirectory("Results");
            int numberOfBricks = data.NumberOfBricks;
            if (data.OddNumberOfBricks) numberOfBricks++;
            string path = string.Format(@"Results\{0}-1.png", numberOfBricks);

            using (FileStream outStream = new FileStream(path, FileMode.Create))
            {
                // Use png encoder for our data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }

            // Restore previously saved layout
            canvas.LayoutTransform = transform;
        }
        private void SaveImage2()
        {

            // Rect rect = new Rect(canvas.Margin.Left, canvas.Margin.Top, canvas.ActualWidth, canvas.ActualHeight + 60);
            Rect rect = new Rect(canvas.Margin.Left, canvas.Margin.Top, canvas.ActualWidth, canvas.ActualHeight);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
              (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);

            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();
            Directory.CreateDirectory("Results");
            string path = string.Format(@"Results\{0}-2.png", data.NumberOfBricks.ToString());
            System.IO.File.WriteAllBytes(path, ms.ToArray());
            Console.WriteLine("Done");
        }
    }
}
