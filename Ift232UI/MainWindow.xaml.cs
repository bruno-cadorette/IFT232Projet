using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Core;
using Core.Army;
using Core.Buildings;
using Core.Technologies;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using Core.Map;
using System.Windows.Media;

namespace Ift232UI
{
    /// <summary>
    ///     Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Game Game;
        private bool boughtResourcesIsLoaded;
        private bool comboBoxBuildingIsLoaded;
        private bool listBoxBuildingIsLoaded;
        private bool soldResourcesIsLoaded;
        private bool technologySelectedIsResearched;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Inscription inscription = new Inscription();
            inscription.ShowDialog();
            Game = inscription.GetGame();
            if (Game != null)
            {
                Update();
                foreach (var player in Game.Players)
                {
                    Players.Items.Add(player.playerName);
                }
                Turns.Content = Game.TurnIndex;
                Update();
                Players.SelectedIndex = Game.PlayerIndex;
                Cities.Content = Game.CurrentPlayer.CurrentCity;
                UnitBox.ItemsSource = ArmyFactory.GetInstance().Soldiers();
            }
            else Close();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void SetPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private void btnNewCity_Click(object sender, RoutedEventArgs e)
        {
            if (Game.CurrentPlayer.CurrentCity.Ressources[ResourcesType.Population] > 500)
            {
                Game.CreateCity(tbNewCity.Text);
                MessageBox.Show("Ville créée !");
            }
            else
            {
                MessageBox.Show("Vous devez avoir plus de population pour créer une nouvelle ville.");
            }
        }

        private void Players_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Players.SelectedIndex = Game.PlayerIndex;
            Update();
        }

        private void NextTurn_Click(object sender, RoutedEventArgs e)
        {
            var turnText = Game.NextTurn();


            if (turnText.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, turnText));
            }


            if (Game.HasWin() || Game.HasLost())
            {
                Application.Current.Shutdown();
            }
            else
            {
                Update();
            }
        }

        private void Update()
        {
            Turns.Content = Game.TurnIndex;
            Players.SelectedIndex = Game.PlayerIndex;
            Cities.Content = Game.CurrentPlayer.CurrentCity;
            UnitBox.ItemsSource = ArmyFactory.GetInstance().Soldiers();
            CurrentUnit.ItemsSource = Game.CurrentPlayer.CurrentCity.Army.Where(n => !n.Type.InConstruction);
            TabArmy.IsEnabled = Game.CurrentPlayer.CurrentCity.FinishedBuildings.Any(t => t is Casern);
            TabTrade.IsEnabled = Game.CurrentPlayer.CurrentCity.FinishedBuildings.Any(t => t is Market);
            UpdateRessource();
            UpdateTechnologyTab();
            UpdateMap();
            if (Listboxdereve.IsLoaded && Listboxdereve.SelectedItem != null)
            {
                var currentValue = Listboxdereve.SelectedItem.ToString();
                tnbrbat.Text =
                    Game.CurrentPlayer.CurrentCity.CountBuilding(currentValue, false)
                        .ToString(CultureInfo.InvariantCulture);
                NBProdTextBox.Text =
                    Game.CurrentPlayer.CurrentCity.CountBuilding(currentValue, true)
                        .ToString(CultureInfo.InvariantCulture);
            }
        }

        private IEnumerable<Tuple<Position, Button>> CreateMap()
        {
            var lastPosition = new Position(0, 0);
            var count = Game.WorldMap.Count();
            foreach (var item in Game.WorldMap)
            {
                foreach (var space in FillSpaces(lastPosition, item.Key))
                {
                    yield return space;
                }
                lastPosition = NextPosition(item.Key);
                if (item.Value is City)
                {
                    var city = item.Value as City;
                    yield return Tuple.Create(item.Key, new Button()
                    {
                        Content = city.Name + " " + city.Army.Size,
                        Background = Brushes.Gold
                    });
                }
                else if (item.Value is Armies)
                {
                    yield return Tuple.Create(item.Key, new Button()
                    {
                        Content = (item.Value as Armies).Size,
                        Background = Brushes.Red
                    });
                }
                else
                {
                    yield return Tuple.Create(item.Key, new Button()
                    {
                        Content = "wtf"
                    });
                }
            }
            foreach (var space in FillSpaces(lastPosition, WorldMap.MaxBound))
            {
                yield return space;
            }
            yield return Tuple.Create(WorldMap.MaxBound, new Button()
                {
                    Content = "nature"
                });
        }
        private IEnumerable<Tuple<Position, Button>> FillSpaces(Position current, Position goal)
        {
            var next = current;
            while (next != goal)
            {
                yield return Tuple.Create(next, new Button()
                {
                    Content = "nature"
                });
                next = NextPosition(next);
            }
        }
        private Position NextPosition(Position current)
        {
            return Game.WorldMap.Length == current.Y ? new Position(current.X + 1, 0) : new Position(current.X, current.Y + 1);
        }

        private void UpdateMap()
        {
            Map.Children.Clear();
            var a = CreateMap();
            var c = a.Count();
            foreach (var button in a)
            {
                Map.Children.Add(button.Item2);
            }
        }

        private void getCities_Click(object sender, RoutedEventArgs e)
        {
            Game.CurrentPlayer.NextCity();
            Update();
        }

        private void cbSelectBuilding_Loaded(object sender, RoutedEventArgs e)
        {
            if (!comboBoxBuildingIsLoaded)
            {
                var buildings = BuildingFactory.GetInstance().Buildings().ToArray();
                foreach (var building in buildings)
                {
                    cbSelectBuilding.Items.Add(building.Name);
                }
                var firstBuilding = buildings.FirstOrDefault();
                if (firstBuilding != null)
                    cbSelectBuilding.SelectedValue = firstBuilding.Name;
            }
            comboBoxBuildingIsLoaded = true;
        }

        private void cbSelectBuilding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var buildings = BuildingFactory.GetInstance().Buildings().ToArray();
            var currentValue = (sender as ComboBox).SelectedItem.ToString();
            Building currentBuilding = buildings.First(n => n.Name == currentValue);
            tbBuildingDatas.Text = currentBuilding.Description;
            tbBuildingDatas.Text += currentBuilding.Requirement.toString();
            tbBuildingDatas.Text += " Nombre de tours nécessaires : ";
            tbBuildingDatas.Text += BuildingFactory.GetInstance().GetBuilding(currentBuilding.ID).TurnsLeft;
        }

        private void btnNewBuilding_Click(object sender, RoutedEventArgs e)
        {
            if (Game.CurrentPlayer.CurrentCity.AddBuilding(cbSelectBuilding.SelectedIndex) != null)
            {
                MessageBox.Show("Bâtiment créé !!!");
            }
            else
            {
                MessageBox.Show("Le bâtiment n'a pu être créé faute de ressources!");
            }
            Update();
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (!listBoxBuildingIsLoaded)
            {
                var buildings = BuildingFactory.GetInstance().Buildings().ToArray();
                foreach (var building in buildings)
                {
                    Listboxdereve.Items.Add(building.Name);
                }
            }
            listBoxBuildingIsLoaded = true;
        }

        private void Listboxdereve_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void SoldResources_Loaded(object sender, RoutedEventArgs e)
        {
            ResourcesType[] listeEchange =
            {
                ResourcesType.Wood, ResourcesType.Gold, ResourcesType.Meat,
                ResourcesType.Rock
            };
            if (!soldResourcesIsLoaded)
            {
                foreach (var elmt in listeEchange)
                {
                    SoldResources.Items.Add(elmt.ToString());
                }
                SoldResources.SelectedValue = listeEchange.FirstOrDefault().ToString();
            }
            soldResourcesIsLoaded = true;
        }

        private void BoughtResources_Loaded(object sender, RoutedEventArgs e)
        {
            ResourcesType[] listeEchange = { ResourcesType.Wood, ResourcesType.Meat, ResourcesType.Rock };
            if (!boughtResourcesIsLoaded)
            {
                foreach (var elmt in listeEchange)
                {
                    BoughtResources.Items.Add(elmt.ToString());
                }
                BoughtResources.SelectedValue = listeEchange.FirstOrDefault().ToString();
            }
            boughtResourcesIsLoaded = true;
        }

        public void UpdateMarketUi()
        {
            if (SoldResources.SelectedItem == null || BoughtResources.SelectedItem == null)
                return;
            int val1 = FirstValue.Value.HasValue ? FirstValue.Value.Value : 0;
            String currentValue = SoldResources.SelectedItem.ToString();
            Market m = (Market)Game.GetMarket();
            if (m == null)
                return;
            var boughtType = Resource.Name.First(n => n.Value == BoughtResources.SelectedItem.ToString());

            var soldType = Resource.Name.First(n => n.Value == SoldResources.SelectedItem.ToString());

            int val2 = m.Trade(val1, soldType.Key, boughtType.Key);
            SecondValue.Text = val2.ToString(CultureInfo.InvariantCulture);

            FirstValue.Maximum = Game.CurrentPlayer.CurrentCity.Ressources[soldType.Key];
        }

        private void SoldResources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarketUi();
        }

        private void BoughtResources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarketUi();
        }

        private void ValidateTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (!FirstValue.Value.HasValue)
                return;

            var soldType = Resource.Name.First(n => n.Value == SoldResources.SelectedItem.ToString());
            var boughtType = Resource.Name.First(n => n.Value == BoughtResources.SelectedItem.ToString());

            int qty = FirstValue.Value.Value;

            bool done = ((Market)Game.GetMarket()).Achat(Game.CurrentPlayer.CurrentCity, qty, soldType.Key,
                boughtType.Key);
            if (done)
            {
                MessageBox.Show("Votre échange a bien eu lieu !!!");
            }
            else
            {
                MessageBox.Show("Votre échange n'a pas eu lieu !!!");
            }


            Update();
        }


        private void Resourcesvilles_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRessource();
        }

        private void UpdateRessource()
        {
            lbResGold.Content = Game.CurrentPlayer.CurrentCity.Ressources[ResourcesType.Gold];
            lbResMeat.Content = Game.CurrentPlayer.CurrentCity.Ressources[ResourcesType.Meat];
            lbResWood.Content = Game.CurrentPlayer.CurrentCity.Ressources[ResourcesType.Wood];
            lbResRock.Content = Game.CurrentPlayer.CurrentCity.Ressources[ResourcesType.Rock];
            lbResPop.Content = Game.CurrentPlayer.CurrentCity.Ressources[ResourcesType.Population];
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (SoldierQuantityBox.Value.HasValue)
            {
                int armyTypeId = ((Soldier)UnitBox.SelectedItem).ID;
                int quantity = SoldierQuantityBox.Value.Value;
                for (var i = 0; i < quantity; i++)
                {
                    if (!Game.CurrentPlayer.CurrentCity.AddArmy(armyTypeId))
                    {
                        MessageBox.Show("Pas assez de ressource pour en produire!", "Production échouée",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                }
                Update();
            }
        }

        private void btnAttaquer(object sender, RoutedEventArgs e)
        {
            //CurrentUnit.SelectedItems
            MessageBox.Show("Not implemented yet. Sorry Fortin.");
        }

        private void FirstValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateMarketUi();
        }

        private void UpdateTechnologyTab()
        {
            TechnologyToDo.SelectedIndex = 0;
            ApplyCountSlider.Value = 0;
            ApplyCountSlider.IsEnabled = false;
            Dictionary<int, Technology> tech = new Dictionary<int, Technology>();
            bool isDone = false;
            foreach (var each in TechnologyFactory.GetInstance().Technologies())
            {
                foreach (var each2 in Game.CurrentPlayer.ResearchedTech)
                {
                    if (each.ID == each2.ID)
                    {
                        isDone = true;
                    }
                }
                if (!isDone)
                {
                    tech.Add(each.ID, each);
                }
                isDone = false;
            }
            if (tech.Count > 0)
                TechnologyToDo.ItemsSource = tech.Select(n => n.Value);
            else
                TechnologyToDo.ItemsSource = null;
            TechnologyDone.ItemsSource = Game.CurrentPlayer.ResearchedTech;
        }


        private void TechnologyToDo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var technology = (Technology)TechnologyToDo.SelectedItem;
            if (technology == null)
            {
                return;
            }
            tbDescription.Text = technology.Description;
            tbRequisite.Text = technology.Requirement.toString();
            TechnologyButton.Content = "Rechercher";
        }

        private void UpgradableEntityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = (CountableListItem<UpgradableEntity>)UpgradableEntityList.SelectedItem;
            if (selection != null)
            {
                //UpgradableEntityList.ItemsSource = Enumerable.Empty<UpgradableEntity>();
                ApplyCountSlider.IsEnabled = true;
                ApplyCountSlider.Value = 0;
                ApplyCountSlider.Maximum = selection.Count;
            }
        }

        private void TechnologyButton_Click(object sender, RoutedEventArgs e)
        {
            if (technologySelectedIsResearched)
            {
                var technologyDone = (Technology)TechnologyDone.SelectedItem;
                if (technologyDone == null)
                    return;
                var entity = (CountableListItem<UpgradableEntity>)UpgradableEntityList.SelectedItem;
                if (entity != null)
                {
                    Game.CurrentPlayer.CurrentCity.UpgradeEntities(entity.Item, technologyDone,
                        (int)ApplyCountSlider.Value);
                }
                technologySelectedIsResearched = false;
                TechnologyDone.SelectedItem = null;
                UpgradableEntityList.ItemsSource = Enumerable.Empty<UpgradableEntity>();
                Update();
            }
            else
            {
                var technologyToDo = (Technology)TechnologyToDo.SelectedItem;
                if (technologyToDo == null)
                    return;
                if (Game.CurrentPlayer.ResearchTechnology(technologyToDo.ID))
                {
                    Update();
                    MessageBox.Show("La recherche de la technologie à été lancée !");
                }
                else
                {
                    MessageBox.Show("Vous n'avez pas les prérequis pour faire cette technologie !");
                }
            }
        }

        private void ApplyCountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderCount.Content = (int)ApplyCountSlider.Value;
        }

        private void TechnologyDone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var technology = (Technology)TechnologyDone.SelectedItem;
            if (technology == null)
            {
                return;
            }
            tbDescription.Text = technology.Description;
            tbRequisite.Text = technology.Requirement.toString();
            if (technology.InConstruction)
                tbRequisite.Text += " Nombre de tours restants :" + technology.TurnsLeft;
            TechnologyButton.Content = "Appliquer";

            if (Game.CurrentPlayer.ResearchedTech.Any(x => !x.InConstruction && x.ID == technology.ID))
            {
                //Technology déjà faite
                ApplyTechGrid.Visibility = Visibility.Visible;
                var buildings = Game.CurrentPlayer.CurrentCity.Buildings
                    .Where(building => building.CanBeAffected(technology))
                    .GroupBy(x => x.ID)
                    .Select(x => new CountableListItem<UpgradableEntity>(x.First(), x.Count()));
                /*
                var soldiers = Game.CurrentPlayer.CurrentCity.Army
                    .Where(soldier => soldier.Type.CanBeAffected(technology))
                    .GroupBy(x => x.Type.ID)
                    .Select(x => new CountableListItem<UpgradableEntity>(x.First(), x.Count()));*/
                //Quand les ID vont être unique
                var entities = Game.CurrentPlayer.CurrentCity.Buildings.Cast<UpgradableEntity>()
                    .Concat(Game.CurrentPlayer.CurrentCity.Army.Select(x => x.Type))
                    .Where(entity => entity.CanBeAffected(technology))
                    .GroupBy(x => x.ID)
                    .Select(x => new CountableListItem<UpgradableEntity>(x.First(), x.Count()));

                UpgradableEntityList.ItemsSource = buildings;//.Concat(soldiers);
                technologySelectedIsResearched = true;
            }
            else
            {
                technologySelectedIsResearched = false;
            }
        }

        private void UnitBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ArmyRequirementUpdate();
        }

        private void ArmyRequirementUpdate()
        {
            var army = (Soldier)UnitBox.SelectedItem;
            if (army == null)
                return;



            if (SoldierQuantityBox.Value.HasValue)
            {
                int number = SoldierQuantityBox.Value.Value;
                ArmyRequirement.Content = "";
                ArmyRequirement.Content += "Or : " + army.Requirement.Resources[ResourcesType.Gold] * number;
                ArmyRequirement.Content += " / Viande : " + army.Requirement.Resources[ResourcesType.Meat] * number;
                ArmyRequirement.Content += " / Bois : " + army.Requirement.Resources[ResourcesType.Wood] * number;
                ArmyRequirement.Content += " / Roche : " + army.Requirement.Resources[ResourcesType.Rock] * number;
                ArmyRequirement.Content += " / Population : " +
                                           army.Requirement.Resources[ResourcesType.Population] * number;
            }

        }

        private void SoldierQuantityBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SoldierQuantityBox.Value < 0)
                SoldierQuantityBox.Value = 0;
            ArmyRequirementUpdate();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog window = new OpenFileDialog();
            window.Filter = "save file|*.sav";
            window.Title = "Séléctionnez le fichier de sauvegarde.";
            window.CheckFileExists = false;
            if (true == window.ShowDialog())
            {
                Game.Save(window.FileName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btnSave_Click(sender, e);
            Close();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            Game = UiTools.Load();
            Update();
        }
    }
}