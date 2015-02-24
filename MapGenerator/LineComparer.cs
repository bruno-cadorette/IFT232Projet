using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MapGenerator
{
    internal class LineComparer : IEqualityComparer<LineGeometry>
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
            return x.StartPoint + ";" + x.EndPoint == y.StartPoint + ";" + y.EndPoint || x.EndPoint + ";" + x.StartPoint == y.StartPoint + ";" + y.EndPoint;
        }

        public int GetHashCode(LineGeometry line)
        {
            return (line.StartPoint.X.GetHashCode() * width + line.StartPoint.Y.GetHashCode()) ^
                (line.EndPoint.X.GetHashCode() * width + line.EndPoint.Y.GetHashCode());
        }
    }
}
