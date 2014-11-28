using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjetIft232.Army;
using Xceed.Wpf.Toolkit;
using ProjetIft232;
using ProjetIft232.Buildings;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace Ift232UI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private void SetPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public Game Game;
        private bool comboBoxBuildingIsLoaded = false;
        private bool listBoxBuildingIsLoaded = false;
        private bool soldResourcesIsLoaded = false;
        private bool boughtResourcesIsLoaded = false;

        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            Game = new Game();
            Inscription inscription = new Inscription(Game);
            inscription.ShowDialog();
            foreach(var player in Game.Players){
                Players.Items.Add(player.playerName);
            }
            
            Update();
            Players.SelectedIndex = Game.PlayerIndex;
            Cities.Content = Game.CurrentPlayer.CurrentCity;
            UnitBox.ItemsSource = ArmyLoader.GetInstance().Soldiers();
        }


        private void btnNewCity_Click(object sender, RoutedEventArgs e)
        {
            if (Game.CurrentPlayer.CurrentCity.Ressources.get("Population")>500)
            {
                Game.CreateCity(tbNewCity.Text);
                labelPopup.Content = "Ville créée !";
                popup.IsOpen = true;
                popup.StaysOpen = false;
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
            string turnText = "";
            turnText = Game.NextTurn();
            Turns.Content = Game.TourIndex;
            Update();
            if (turnText != "" && turnText != "Vous avez perdu!  :'( " && turnText != "Vous avez gagné!")
            {
                MessageBox.Show(turnText);
            }
            else if (turnText == "Vous avez perdu!  :'( ")
            {
                MessageBox.Show(turnText + "le jeu va quitter de manière Brutale");
                Application.Current.Shutdown();
            }
            else if (turnText == "Vous avez gagné!")
            {
                MessageBox.Show(turnText + "le jeu va quitter de manière Brutale");
                Application.Current.Shutdown();
            }
        }

        private void Update()
        {
            Players.SelectedIndex = Game.PlayerIndex;
            Cities.Content = Game.CurrentPlayer.CurrentCity;
            UnitBox.ItemsSource = ArmyLoader.GetInstance().Soldiers();
            CurrentUnit.ItemsSource = Game.CurrentPlayer.CurrentCity.army.getUnits().Where(n => !n.InConstruction);
            TabArmy.IsEnabled = Game.CurrentPlayer.CurrentCity.FinishedBuildings.Any(t => t.ID == 5);
            TabTrade.IsEnabled = Game.CurrentPlayer.CurrentCity.FinishedBuildings.Any(t => t is Market);
            UpdateRessource();
            if (Listboxdereve.IsLoaded && Listboxdereve.SelectedItem != null)
            {
                var currentValue = Listboxdereve.SelectedItem.ToString();
                tnbrbat.Text = Game.CurrentPlayer.CurrentCity.CountBuilding(currentValue, false).ToString();
                NBProdTextBox.Text = Game.CurrentPlayer.CurrentCity.CountBuilding(currentValue, true).ToString();
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
                var buildings = BuildingLoader.GetInstance().Buildings().ToArray();
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
            var buildings = BuildingLoader.GetInstance().Buildings().ToArray();
            var currentValue = (sender as ComboBox).SelectedItem.ToString();
            tbBuildingDatas.Text = buildings.First(n => n.Name == currentValue).Description;
        }

        private void btnNewBuilding_Click(object sender, RoutedEventArgs e)
        {
            if (Game.CurrentPlayer.CurrentCity.AddBuilding(cbSelectBuilding.SelectedIndex))
            {
                labelPopup.Content = "Bâtiment créé !!!";
                popup.IsOpen = true;
                popup.StaysOpen = false;

            }
            else 
            {
                labelPopup.Content = "Le bâtiment n'a pu être créé faute de ressources!";
                popup.IsOpen = true;
                popup.StaysOpen = false;

            }
            Update();

        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (!listBoxBuildingIsLoaded)
            {
                var buildings = BuildingLoader.GetInstance().Buildings().ToArray();
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
            ResourcesType[] listeEchange = { ResourcesType.Wood, ResourcesType.Gold,  ResourcesType.Meat, ResourcesType.Rock };
            if(!soldResourcesIsLoaded)
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
            Market m = (Market) Game.getMarket();
            if (m == null)
                return;
            var boughtType = Resource.Name.First(n => n.Value == BoughtResources.SelectedItem.ToString());

            var soldType = Resource.Name.First(n => n.Value == SoldResources.SelectedItem.ToString());

            int val2 = m.Trade(val1, soldType.Key, boughtType.Key);
            SecondValue.Text = val2.ToString();

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

            bool done = ((Market)Game.getMarket()).Achat(Game.CurrentPlayer.CurrentCity, qty, soldType.Key, boughtType.Key);
            if (done)
            {
                labelPopup.Content = "Votre échange a bien eu lieu !!!";
                popup.IsOpen = true;
                popup.StaysOpen = false;
            }
            else
            {
                labelPopup.Content = "Votre échange n'a pas eu lieu !!!";
                popup.IsOpen = true;
                popup.StaysOpen = false;
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
                int armyTypeId = ((ArmyUnit) UnitBox.SelectedItem).ID;
                int quantity = SoldierQuantityBox.Value.Value;
                for (var i = 0; i < quantity; i++)
                {
                    if (!Game.CurrentPlayer.CurrentCity.AddArmy(armyTypeId))
                    {
                        MessageBox.Show("Pas assez de ressource pour en produire!", "Production échoué", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                }
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
    }
}
