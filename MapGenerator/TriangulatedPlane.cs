using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapGenerator
{
    public class TriangulatedPlane : IEnumerable<Triangle>
    {
        private IEnumerable<Triangle> triangles;
        public int Height { get; private set; }
        public int Width { get; private set; }
        public IEnumerator<Triangle> GetEnumerator()
        {
            return triangles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public static TriangulatedPlane TriangulationFromPoints(RandomPointsPlane plane)
        {
            Triangle superTriangle = new Triangle()
            {
                A = new Point(-plane.Width / 2, 0),
                B = new Point(plane.Width / 2, plane.Height * 2),
                C = new Point(plane.Width + plane.Width / 2, 0)
            };
            var triangulation = new HashSet<Triangle>()
            {
                superTriangle
            };

            foreach (var point in plane)
            {
                var badTriangles = triangulation.Where(triangle => triangle.IsPointInCircumcircle(point)).ToArray();
                foreach (var badTriangle in badTriangles)
                {
                    triangulation.Remove(badTriangle);
                }
                var polygon = badTriangles
                    .SelectMany(x => x.Edges).GroupBy(e => e, new LineComparer(plane.Width))
                    .Where(x => x.Count() == 1).Select(x => new Triangle(point, x.Key));
                foreach (var triangle in polygon)
                {
                    triangulation.Add(triangle);
                }
            }
            return new TriangulatedPlane()
            {
                triangles = triangulation.Where(tri => tri.Points.All(x => !superTriangle.Points.Contains(x))),
                Height = plane.Height,
                Width = plane.Width
            };
        }
    }
}
