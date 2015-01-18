using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameHelper
{
    public class TileSetGenerator
    {
        public string Path { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        private int borderSize;
        public TileSetGenerator(string path, int height, int width, int borderSize = 0)
        {
            Path = path;
            Height = height;
            Width = width;
            this.borderSize = borderSize;
        }
        public IEnumerable<BitmapSource> GetTiles()
        {
            BitmapImage tileset = new BitmapImage(new Uri(Path, UriKind.Relative));
            for (int y = 0; y < tileset.PixelHeight - borderSize; y += Height + borderSize)
            {
                for (int x = 0; x < tileset.PixelWidth - borderSize; x += Width + borderSize)
                {
                    Int32Rect area = new Int32Rect(x, y, Width, Height);
                    yield return new CroppedBitmap(tileset, area);

                }
            }
        }
        public Dictionary<int, BitmapSource> GetUsedTiles(IEnumerable<int> tilesId)
        {
            BitmapImage tileset = new BitmapImage(new Uri(Path, UriKind.Relative));
            return tilesId.Select(i =>
            {
                int x = (i % (tileset.PixelWidth / Width))*(Width+borderSize);
                int y = (i / (tileset.PixelHeight / Height) +1) * (Height+borderSize);
                Int32Rect area = new Int32Rect(x, y, Width, Height);
                return new { id = i, tile = new CroppedBitmap(tileset, area) };
            }).ToDictionary(x => x.id, x => (BitmapSource)x.tile);

        }
    }
}
