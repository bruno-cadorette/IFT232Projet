using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MapGenerator
{
    public class VoronoiDiagram : IEnumerable<LineGeometry>
    {
        private IEnumerable<LineGeometry> lines;
        public static VoronoiDiagram DiagramFromTriangulation(TriangulatedPlane triangles)
        {
            return new VoronoiDiagram()
            {
                lines = triangles.SelectMany(triangle =>
                {
                    return triangles.Where(t => triangle.Edges.Any(x => t.Edges.Contains(x, new LineComparer(triangles.Width))))
                        .Select(t => new LineGeometry(triangle.Circumcircle.Center, t.Circumcircle.Center));
                })
            };
        }

        public IEnumerator<LineGeometry> GetEnumerator()
        {
            return lines.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
