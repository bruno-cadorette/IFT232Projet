using GameHelper;
using Core.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Core;
using System.Windows.Media.Imaging;
using Core.Configuration;

namespace Ift232UI
{
    public class MapViewModel : BindableBase
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

        public int TurnIndex
        {
            get
            {
                return game.TurnIndex;
            }
        }
        public Position SelectedCell { get; set; }
        public ICommand SelectCell { get; private set; }
        public ICommand SettleDown { get; private set; }
        public ICommand AdministrateCity { get; private set; }
        public ICommand SpawnArmy { get; private set; }
        public ICommand NextTurn { get; private set; }
        private Action<City> openCityWindow;
        private Game game;
        private Dictionary<int, BitmapSource> tiles;
        private bool SpawningMode = false;
        public MapViewModel(Game game, Action<City> openCityWindow)
        {
            this.game = game;
            this.openCityWindow = openCityWindow;
            var usedLandId = GameConfig.Instance.WorldMapLandscape.Lands.Select(x=>x.ID).Distinct();
            tiles = new TileSetGenerator(GameConfig.Instance.WorldMapLandscape.TileSet, 32, 32).GetUsedTiles(usedLandId);
            var unSelect = new ActionCommand(() => SelectedCell = null);
            var updateMap = new ActionCommand(() =>
            {
                Tiles.Clear();

                foreach (var tile in MapItems())
                {
                    Tiles.Add(tile);
                }
            });
            Tiles = new ObservableCollection<MapItemViewModel>(MapItems());
            SelectCell = new RelayCommand<int>(i =>
                {
                    var position = game.WorldMap.NthPosition(i);
                    if(SpawningMode)
                    {
                        SpawningMode = false;
                    }
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
                    UpdateCommands();
                });
            SettleDown = new MacroRelayCommand<Position>(x => SelectedCell!=null && game.WorldMap[SelectedCell] is IMapItemConverter)
            {
                new ActionCommand(()=>game.WorldMap.ConvertItem(SelectedCell)), 
                unSelect,
                updateMap
            };
            SpawnArmy = new ActionCommand(()=>SpawningMode=true,()=>SelectedCell!=null&&game.WorldMap[SelectedCell] is IMovableItemSpawner);
            NextTurn = new MacroRelayCommand<Position>()
            {
                new ActionCommand(() => 
                    {
                        game.NextTurn();
                        this.OnPropertyChanged("TurnIndex");
                    }),
                updateMap
            };

            AdministrateCity = new MacroRelayCommand<Position>()
            {
                new ActionCommand(() => openCityWindow(game.WorldMap[SelectedCell] as City),
                    () => SelectedCell!=null && game.WorldMap[SelectedCell] is City),
                unSelect
            };
        }
        private IEnumerable<MapItemViewModel> MapItems()
        {
            return game.WorldMap.GetAllCellsForPlayer(game.CurrentPlayer.ID).Select(x => new MapItemViewModel(x, tiles[x.Land.ID]));
        }
        private void UpdateCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
