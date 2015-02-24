using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapGenerator
{
    public class RandomPointsPlane : IEnumerable<Point>
    {
        private Random random;
        public int Height { get; private set; }
        public int Width { get; private set; }
        private IEnumerable<Point> points;
        

        public RandomPointsPlane(int height, int width, int n)
            : this(new Random().Next(int.MaxValue), height, width, n)
        {

        }
        public RandomPointsPlane(int seed, int height, int width, int n)
        {
            random = new Random(seed);
            Height = height;
            Width = width;
            points = GenerateRandomPoints(n);
        }
        

        private IEnumerable<Point> GenerateRandomPoints(int n)
        {
            for (int i = 0; i < n; i++)
            {
                yield return new Point(RandomValue(Height), RandomValue(Width));
            }
        }
        private double RandomValue(int max)
        {
            return random.NextDouble() * max;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
