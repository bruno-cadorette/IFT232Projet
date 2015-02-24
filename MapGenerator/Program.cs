using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            DrawIslandDebug(250);
        }

        static void DrawIslandDebug(int n)
        {
            var plane = new RandomPointsPlane(4096, 4096, 250);
            DrawStepDebug(TriangulatedPlane.TriangulationFromPoints(plane));
        }
        static void DrawStepDebug(TriangulatedPlane plane)
        {
            Pen blackPen = new Pen(Brushes.Black, 3);
            Pen orangePen = new Pen(Brushes.Orange, 3);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            foreach (var line in plane.SelectMany(x=>x.Edges))
            {
                drawingContext.DrawLine(orangePen, line.StartPoint, line.EndPoint);
            }
            foreach (var line in VoronoiDiagram.DiagramFromTriangulation(plane))
            {
                drawingContext.DrawLine(blackPen, line.StartPoint, line.EndPoint);
            }
            drawingContext.Close();

            RenderTargetBitmap rtb = new RenderTargetBitmap(4096, 4096, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            // Encoding the RenderBitmapTarget as a PNG file.
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            using (Stream stm = File.Create("new.png"))
            {
                png.Save(stm);
            }

        }
    }
}
