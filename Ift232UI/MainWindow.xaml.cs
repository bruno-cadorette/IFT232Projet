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
using Xceed.Wpf.Toolkit;
using ProjetIft232;
using ProjetIft232.Buildings;

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

        string _test1;
        public string test1 { 
            get
        {
            return _test1;
        }
        set
        {
            _test1 = value;
            SetPropertyChanged("test1");
        }
        }

        Timer t;
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
            Players.SelectedIndex = Game.PlayerIndex;
            Cities.Content = Game.CurrentPlayer.CurrentCity;
        }


        private void btnNewCity_Click(object sender, RoutedEventArgs e)
        {
            Game.CreateCity(tbNewCity.Text);
            labelPopup.Content = "Ville créée !";
            popup.IsOpen = true;
            popup.StaysOpen = false;
        }

        private void Players_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Players.SelectedIndex = Game.PlayerIndex;
        }

        private void NextTurn_Click(object sender, RoutedEventArgs e)
        {
            Game.NextTurn();
            Turns.Content = Game.TourIndex;
            Players.SelectedIndex = Game.PlayerIndex;
            Cities.Content= Game.CurrentPlayer.CurrentCity;
        }

        private void getCities_Click(object sender, RoutedEventArgs e)
        {
            Game.CurrentPlayer.NextCity();
            Cities.Content = Game.CurrentPlayer.CurrentCity;
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
                cbSelectBuilding.SelectedValue = buildings.FirstOrDefault().Name;
            }
            comboBoxBuildingIsLoaded = true;
        }

        private void cbSelectBuilding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var buildings = BuildingLoader.GetInstance().Buildings().ToArray();
            var currentValue = (sender as ComboBox).SelectedItem.ToString();
            tbBuildingDatas.Text = buildings.Where(n => n.Name == currentValue).First().Description;
        }

        private void btnNewBuilding_Click(object sender, RoutedEventArgs e)
        {
            if (Game.CurrentPlayer.CurrentCity.AddBuilding(cbSelectBuilding.SelectedIndex))
            {
                Game.CurrentPlayer.CurrentCity.AddBuilding(cbSelectBuilding.SelectedIndex);
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
            var currentValue = (sender as ListBox).SelectedItem.ToString();
            tnbrbat.Text = Game.CurrentPlayer.CurrentCity.CountBuilding(currentValue).ToString();
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

        private void FirstValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMarketUI();
        }

        public void UpdateMarketUI()
        {
            try
            {
                int val1 = int.Parse(FirstValue.Text);
                String currentValue = SoldResources.SelectedItem.ToString();
                Market m = (Market)Game.getMarket();
                var boughtType = Resource.Name.First(n => n.Value == BoughtResources.SelectedItem.ToString());
                if (currentValue == "Gold")
                {
                    int val2 = m.Conversion(val1, boughtType.Key);
                    SecondValue.Text = val2.ToString();
                }
                else
                {
                    var soldType = Resource.Name.First(n => n.Value == SoldResources.SelectedItem.ToString());
                    int val2 = m.Trade(val1, soldType.Key, boughtType.Key);
                    SecondValue.Text = val2.ToString();
                }
                
            }
            catch (NullReferenceException)
            {
                labelPopup.Content = "Votre marché n'est pas créé";
                popup.IsOpen = true;
                popup.StaysOpen = false;
            }
            catch (FormatException)
            {
                
            }
        }

        private void SoldResources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarketUI();
        }

        private void BoughtResources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarketUI();
        }

        private void ValidateTransaction_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                String currentValue = SoldResources.SelectedItem.ToString();
                int val1 = int.Parse(FirstValue.Text);
                if (currentValue == "Gold")
                {
                    var boughtType = Resource.Name.First(n => n.Value == BoughtResources.SelectedItem.ToString());
                    bool done =((Market)Game.getMarket()).Achat(Game.CurrentPlayer.CurrentCity, boughtType.Key, val1);
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
                }
          
         
            }
            catch (NullReferenceException)
            {
                labelPopup.Content = "Votre marché n'est pas créé";
                popup.IsOpen = true;
                popup.StaysOpen = false;
            }
            catch (FormatException)
            {
                labelPopup.Content = "On attend une valeur pour l'échange";
                popup.IsOpen = true;
                popup.StaysOpen = false;

            }


        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Resourcesvilles_Loaded(object sender, RoutedEventArgs e)
        {
            string resources ="";
            foreach(var i in Resource.Name){
                resources += Game.CurrentPlayer.CurrentCity.Ressources.get(i.Value);
                if (i.Key != ResourcesType.End - 1)
                    resources += "/";
            }
            Resourcesvilles.Text = resources;

        }

      
      



    


    }
}
