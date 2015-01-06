using GameHelper;
using Core.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Core;

namespace Ift232UI
{
    public class MapViewModel
    {
        public ObservableCollection<MapItemViewModel> Tiles { get; set; }
        public Position MaxBound
        {
            get
            {
                return WorldMap.MaxBound;
            }
        }
        public Position MapSize
        {
            get
            {
                return new Position(WorldMap.MaxBound.X * 32, WorldMap.MaxBound.Y * 32);
            }
        }
        public Position SelectedCell { get; set; }
        public ICommand SelectCell { get; private set; }
        public ICommand SettleGround { get; private set; }
        public ICommand AdministrateCity { get; private set; }
        public ICommand NextTurn { get; private set; }
        private Game game;

        public MapViewModel(Game game)
        {
            this.game = game;
            var unSelect = new RelayCommand<Position>(x => SelectedCell = null);
            var updateMap = new RelayCommand<Position>(_ =>
            {
                Tiles.Clear();
                foreach (var tile in game.WorldMap.Select(x=>new MapItemViewModel(x)))
                {
                    Tiles.Add(tile);
                }
            });
            Tiles = new ObservableCollection<MapItemViewModel>(game.WorldMap.Select(x=>new MapItemViewModel(x)));
            SelectCell = new RelayCommand<int>(i =>
                {
                    int x = i % MaxBound.X;
                    int y = i / MaxBound.Y;
                    var position = new Position(x, y);
                    if (SelectedCell != null)
                    {
                        game.WorldMap.SetMove(SelectedCell, position);
                        unSelect.Execute(position);
                        updateMap.Execute(position);
                    }
                    else
                    {
                        SelectedCell = position;
                    }
                },
                x => SelectedCell == null || game.WorldMap[SelectedCell] is MovableItem);
            SettleGround = new MacroRelayCommand<Position>(x => SelectedCell != null)
            {
                new RelayCommand<Position>(game.WorldMap.ConvertItem, x => game.WorldMap[x] is IMapItemConverter),
                unSelect,
                updateMap
            };
            NextTurn = new MacroRelayCommand<Position>()
            {
                new RelayCommand<Position>(x => game.NextTurn()),
                updateMap
            };

            AdministrateCity = new RelayCommand<Position>(x => { }, x => game.WorldMap[x] is City);
        }
    }
}
