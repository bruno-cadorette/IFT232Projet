using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameBuilder.Utility
{
    class TileSetGenerator
    {
        private BitmapImage tileset;
        public TileSetGenerator(string path)
        {
            tileset = new BitmapImage(new Uri(path));
        }
        public IEnumerable<BitmapSource> GetTiles(int height, int width)
        {
            for (int y = 0; y < tileset.PixelHeight; y += height)
            {
                for (int x = 0; x < tileset.PixelWidth; x += width)
                {
                    Int32Rect area = new Int32Rect(x, y, width, height);
                    yield return new CroppedBitmap(tileset, area);
                    
                }
            }
        }
    }
}
