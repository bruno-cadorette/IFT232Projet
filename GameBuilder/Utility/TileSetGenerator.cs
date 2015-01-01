using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameBuilder.Utility
{
    public class TileSetGenerator
    {
        private string path;
        private int height;
        private int width;
        private int borderSize;
        public TileSetGenerator(string path, int height, int width, int borderSize = 0)
        {
            this.path = path;
            this.height = height;
            this.width = width;
            this.borderSize = borderSize;
        }
        public IEnumerable<BitmapSource> GetTiles()
        {
            BitmapImage tileset = new BitmapImage(new Uri(path, UriKind.Relative));
            for (int y = 0; y < tileset.PixelHeight - borderSize; y += height + borderSize)
            {
                for (int x = 0; x < tileset.PixelWidth - borderSize; x += width + borderSize)
                {
                    Int32Rect area = new Int32Rect(x, y, width, height);
                    yield return new CroppedBitmap(tileset, area);
                    
                }
            }
        }
    }
}
