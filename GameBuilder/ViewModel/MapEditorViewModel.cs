﻿using Core;
using Core.Configuration;
using Core.Map;
using GameHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameBuilder
{
    public class MapEditorViewModel : BindableBase
    {
        public TrulyObservableCollection<LandscapeViewModel> LandscapeTiles { get; set; }
        public TrulyObservableCollection<LandscapeViewModel> LandscapeSelector { get; set; }
        public LandscapeViewModel SelectedLandscape { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int PixelHeight
        {
            get
            {
                return Height * 32;
            }
        }
        public int PixelWidth
        {
            get
            {
                return Width * 32;
            }
        }
        public bool FillMode { get; set; }
        public bool ColorSelector { get; set; }
        public bool EraseMode { get; set; }
        public ICommand GenerateMap { get; private set; }
        public ICommand ChangeLand { get; private set; }
        public ICommand Save { get; private set; }
        public ICommand SelectLandscape { get; private set; }

        public MapEditorViewModel(TileSetGenerator tileSetGenerator, int width, int height)
        {
            Width = width;
            Height = height;
            LandscapeSelector = new TrulyObservableCollection<LandscapeViewModel>(
                tileSetGenerator.GetTiles().Select((x, id) => new LandscapeViewModel(id, x)));
            LandscapeViewModel.DefaultLandscape = LandscapeSelector.First();
            LandscapeTiles = new TrulyObservableCollection<LandscapeViewModel>();
            SelectedLandscape = LandscapeViewModel.DefaultLandscape;
            FillMode = false;
            ChangeLand = new RelayCommand<int>(i =>
                {
                    if (FillMode)
                    {
                        int x = i % Width;
                        int y = i / Height;
                        Fill(x, y, LandscapeTiles[i], SelectedLandscape);
                    }
                    else if (ColorSelector)
                    {
                        SelectedLandscape = LandscapeTiles[i];
                    }
                    else
                    {
                        LandscapeTiles[i] = EraseMode ? LandscapeViewModel.DefaultLandscape : SelectedLandscape;
                    }
                });
            Save = new RelayCommand<object>(_ =>
                {
                    new GameConfigData()
                    {
                        Landscape = new LandscapeConfig(tileSetGenerator.Path, LandscapeTiles.Select(x => new Land()
                        {
                            ID = x.ID
                        }), Width, Height),
                        Entities = Enumerable.Empty<BuildableEntity>()
                    }.Save("newConfig.xml");

                });
            SelectLandscape = new RelayCommand<LandscapeViewModel>(x => SelectedLandscape = x);
        }
        
        private IEnumerable<LandscapeViewModel> TilesGenerator()
        {
            return Enumerable.Repeat(LandscapeViewModel.DefaultLandscape, Height * Width);
        }
        private void ReplaceItem(Coordonate point, LandscapeViewModel item)
        {
            LandscapeTiles[GetIndex(point.X, point.Y)] = item;
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
                if (GetColor(n.X, n.Y) != targetColor)
                {
                    continue;
                }
                Coordonate w = n, e = new Coordonate(n.X + 1, n.Y);
                while ((w.X >= 0) && GetColor(w.X, w.Y) == targetColor)
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
                while ((e.X <= Width - 1) && GetColor(e.X, e.Y) == targetColor)
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

        private LandscapeViewModel GetColor(int x, int y)
        {
            return LandscapeTiles[GetIndex(x, y)];
        }

        private int GetIndex(int x, int y)
        {
            return y * Height + x;
        }
    }
}
