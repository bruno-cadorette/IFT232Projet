using Core.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GameBuilder.MapGenerator
{
    class IslandGenerator
    {
        private Random random;
        private int height;
        private int width;
        public IslandGenerator(int height, int width)
            : this(new Random().Next(int.MaxValue), height, width)
        {

        }
        public IslandGenerator(int seed, int height, int width)
        {
            random = new Random(seed);
            this.height = height;
            this.width = width;
        }


        private class LineComparer : IEqualityComparer<LineGeometry>
        {
            private int width;
            public LineComparer(int width)
            {
                this.width = width;
            }
            /// <summary>
            /// Ugliest way ever, but it work
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(LineGeometry x, LineGeometry y)
            {
                //return x.StartPoint + ";" + x.EndPoint == y.StartPoint + ";" + y.EndPoint || x.EndPoint + ";" + x.StartPoint == y.StartPoint + ";" + y.EndPoint;
                return Math.Ceiling(x.StartPoint.X) == Math.Ceiling(y.StartPoint.X) && Math.Ceiling(x.StartPoint.Y) == Math.Ceiling(y.StartPoint.Y);
            }

            public int GetHashCode(LineGeometry line)
            {
                return (line.StartPoint.X.GetHashCode() * width + line.StartPoint.Y.GetHashCode()) ^
                    (line.EndPoint.X.GetHashCode() * width + line.EndPoint.Y.GetHashCode());
            }
        }

        private IEnumerable<Point> GenerateRandomPoints(int n)
        {

            for (int i = 0; i < n; i++)
            {
                yield return new Point(RandomValue(width), RandomValue(height));
            }
        }
        private double RandomValue(int max)
        {
            return random.NextDouble() * max;
        }

        public IEnumerable<Triangle> Triangulation(int n)
        {
            return Triangulation(GenerateRandomPoints(n));
        }
        public IEnumerable<Triangle> Triangulation(IEnumerable<Point> positions)
        {
            Triangle superTriangle = new Triangle()
            {
                A = new Point(-width / 2, 0),
                B = new Point(width / 2, height * 2),
                C = new Point(width + width / 2, 0)
            };
            var triangulation = new List<Triangle>()
            {
                superTriangle
            };

            foreach (var point in positions)
            {
                var badTriangles = triangulation.Where(triangle => triangle.IsPointInCircumcircle(point)).ToArray();
                var polygon = badTriangles.SelectMany(x => x.Edges).Distinct(new LineComparer(width));
                foreach (var triangle in badTriangles)
                {
                    triangulation.Remove(triangle);
                }
                triangulation.AddRange(polygon.Select(edge => new Triangle(point, edge)));
            }
            return triangulation.Where(tri => tri.Points.All(x=>!superTriangle.Points.Contains(x)));
        }

    }
}
