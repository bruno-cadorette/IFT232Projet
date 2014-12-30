using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GameBuilder
{
    public class MapEditorViewModel
    {
        public TrulyObservableCollection<LandscapeViewModel> LandscapeTiles { get; set; }
        public TrulyObservableCollection<LandscapeViewModel> LandscapeSelector { get; set; }
        public LandscapeViewModel SelectedLandscape { get; set; }
        public int Height
        {
            get
            {
                return 100;
            }
        }
        public int Width
        {
            get
            {
                return 100;
            }
        }
        public int TotalNumberOfItems
        {
            get
            {
                return Height * Width;
            }
        }
        public bool FillMode { get; set; }
        public bool EraseMode { get; set; }
        public ICommand ChangeLand { get; private set; }
        public ICommand SaveLand { get; private set; }

        private int lenght;

        public MapEditorViewModel()
        {
            LandscapeTiles = new TrulyObservableCollection<LandscapeViewModel>(TilesGenerator());
            LandscapeSelector = new TrulyObservableCollection<LandscapeViewModel>(LandscapeGenerator());
            SelectedLandscape = LandscapeSelector[new Random().Next(LandscapeSelector.Count)];
            lenght = (int)Math.Sqrt(LandscapeTiles.Count);
            FillMode = false;
            ChangeLand = new RelayCommand<int>(i =>
                {
                    if (FillMode)
                    {
                        int x = i % Width;
                        int y = i / Height;
                        Fill(x, y, LandscapeTiles[i], SelectedLandscape);
                    }
                    else
                    {
                        ReplaceItem(i, EraseMode ? LandscapeViewModel.DefaultLandscape() : SelectedLandscape);
                    }
                });
        }
        private IEnumerable<LandscapeViewModel> TilesGenerator()
        {
            return Enumerable.Repeat(LandscapeViewModel.DefaultLandscape(), TotalNumberOfItems);
        }
        private void ReplaceItem(int index, LandscapeViewModel item)
        {
            LandscapeTiles.RemoveAt(index);
            LandscapeTiles.Insert(index, item);
            LandscapeTiles[index].Color = item.Color;
        }

        private void Fill(int x, int y, LandscapeViewModel colorTarget, LandscapeViewModel remplacement)
        {
            if (colorTarget == remplacement)
            {
                return;
            }
            if (IsValid(x) && IsValid(y) && GetColor(x, y) == colorTarget)
            {
                ReplaceItem(GetIndex(x, y), remplacement);
                Fill(x, y + 1, colorTarget, remplacement);
                Fill(x, y - 1, colorTarget, remplacement);
                Fill(x + 1, y, colorTarget, remplacement);
                Fill(x - 1, y, colorTarget, remplacement);
            }
        }
        private bool IsValid(int x)
        {
            return 0 <= x && x < lenght;
        }

        private LandscapeViewModel GetColor(int x, int y)
        {
            return LandscapeTiles[GetIndex(x, y)];
        }
        private int GetIndex(int x, int y)
        {

            return y * lenght + x;
        }
        private IEnumerable<LandscapeViewModel> LandscapeGenerator()
        {
            return typeof(Brushes).GetProperties().Select(x => new LandscapeViewModel((x.GetValue(null, null) as SolidColorBrush).Color));
        }
    }
}
