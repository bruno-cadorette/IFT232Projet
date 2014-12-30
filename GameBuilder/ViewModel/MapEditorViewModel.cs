using GameBuilder.Utility;
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
        public bool FillMode { get; set; }
        public bool EraseMode { get; set; }
        public ICommand ChangeLand { get; private set; }
        public ICommand SaveLand { get; private set; }
        public ICommand SelectLandscape { get; private set; }

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
                        LandscapeTiles[i] = EraseMode ? LandscapeViewModel.DefaultLandscape() : SelectedLandscape;
                    }
                });
            SelectLandscape = new RelayCommand<LandscapeViewModel>(x => SelectedLandscape = x);
        }
        private IEnumerable<LandscapeViewModel> TilesGenerator()
        {
            return Enumerable.Repeat(LandscapeViewModel.DefaultLandscape(), Height * Width);
        }
        private void ReplaceItem(Coordonate point, LandscapeViewModel item)
        {
            LandscapeTiles[GetIndex((int)point.X, (int)point.Y)] = item;
        }


        private struct Coordonate
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Coordonate(int x, int y)
                : this()
            {
                X = x;
                Y = y;
            }
        }
        private void Fill(int x, int y, LandscapeViewModel targetColor, LandscapeViewModel replacementColor)
        {
            Queue<Coordonate> q = new Queue<Coordonate>();
            q.Enqueue(new Coordonate(x, y));
            while (q.Count > 0)
            {
                Coordonate n = q.Dequeue();
                if (GetColor(n) != targetColor)
                    continue;
                Coordonate w = n, e = new Coordonate(n.X + 1, n.Y);
                while ((w.X > 0) && GetColor(n) == targetColor)
                {
                    ReplaceItem(w, replacementColor);
                    if ((w.Y > 0) && GetColor(w.X, w.Y - 1) == targetColor)
                    {
                        q.Enqueue(new Coordonate(w.X, w.Y - 1));
                    }

                    if ((w.Y < Height - 1) && GetColor(w.X, w.Y + 1) == targetColor)
                    {
                        q.Enqueue(new Coordonate(w.X, w.Y + 1));
                    }
                    w.X--;
                }
                while ((e.X < Width - 1) && GetColor(e.X, e.Y) == targetColor)
                {
                    ReplaceItem(e, replacementColor);
                    if ((e.Y > 0) && GetColor(e.X, e.Y - 1) == targetColor)
                    {
                        q.Enqueue(new Coordonate(e.X, e.Y - 1));
                    }
                    if ((e.Y < Height - 1) && GetColor(e.X, e.Y + 1) == targetColor)
                    {
                        q.Enqueue(new Coordonate(e.X, e.Y + 1));
                    }
                    e.X++;
                }
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
        private LandscapeViewModel GetColor(Coordonate point)
        {
            return GetColor((int)point.X, (int)point.Y);
        }

        private int GetIndex(int x, int y)
        {
            return y * lenght + x;
        }
        private IEnumerable<LandscapeViewModel> LandscapeGenerator()
        {
            return typeof(Brushes).GetProperties().Select(x => new LandscapeViewModel((x.GetValue(null, null) as SolidColorBrush).Color)).OrderBy(x => x.Color.ToString());
        }
    }
}
