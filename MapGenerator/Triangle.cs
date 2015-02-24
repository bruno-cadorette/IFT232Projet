using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MapGenerator
{
    public class Triangle
    {
        private EllipseGeometry circumcircle = null;
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
        public Triangle()
        {
            
        }
        public Triangle(Point a, LineGeometry edge)
        {
            A = a;
            B = edge.StartPoint;
            C = edge.EndPoint;
        }
        public IEnumerable<Point> Points
        {
            get
            {
                yield return A;
                yield return B;
                yield return C;
            }
        }
        public IEnumerable<LineGeometry> Edges
        {
            get
            {
                yield return new LineGeometry(A, B);
                yield return new LineGeometry(B, C);
                yield return new LineGeometry(C, A);
            }
        }
        public static double LineLength(LineGeometry line)
        {
            return Distance(line.StartPoint, line.EndPoint);
        }
        public static double Distance(Point a, Point b)
        {
            double x = a.X - b.X;
            double y = a.Y - b.Y;
            return Math.Sqrt(x * x + y * y);
        }
        private double CircumcircleRadius()
        {
            var sides = Edges.Select(LineLength).ToArray();
            double a = sides[0];
            double b = sides[1];
            double c = sides[2];
            double num = a * b * c;
            double denum = (a + b + c) * (b + c - a) * (c + a - b) * (a + b - c);
            return num / Math.Sqrt(denum);
        }
        private double CenterHelp(Point p1, Point p2, Point p3, Func<Point, double> f)
        {
            return (p1.X * p1.X + p1.Y * p1.Y) * (f(p2) - f(p3));
        }
        private Point CircumcircleCenter()
        {
            Func<Point, double> fx = p=>p.X;
            Func<Point, double> fy = p=>p.Y;
            var ux = CenterHelp(A, B, C, fy) + CenterHelp(B, C, A, fy) + CenterHelp(C, A, B, fy);
            var uy = CenterHelp(A, C, B, fx) + CenterHelp(B, A, C, fx) + CenterHelp(C, B, A, fx);
            var d = 2 * (A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y));
            return new Point(ux / d, uy / d);
        }
        private EllipseGeometry ComputeCircumcircle()
        {
            var radius = CircumcircleRadius();
            return new EllipseGeometry(CircumcircleCenter(), radius, radius);
        }
        public bool IsPointInCircumcircle(Point point)
        {
            var circle = Circumcircle;
            return Distance(circle.Center, point) <= circle.RadiusX;
        }
        public EllipseGeometry Circumcircle
        {
            get
            {
                if (circumcircle==null)
                {
                    circumcircle = ComputeCircumcircle();
                }
                return circumcircle;
            }
        }
    }
}
