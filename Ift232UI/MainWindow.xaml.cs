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
using ProjetIft232.Technologies;
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
        private bool technologySelectedIsResearched = false;
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
                MessageBox.Show(turnText + "le jeu va quitter de manière brutale");
                Application.Current.Shutdown();
            }
            else if (turnText == "Vous avez gagné!")
            {
                MessageBox.Show(turnText + "le jeu va quitter de manière brutale");
                Application.Current.Shutdown();
            }
        }

        private void Update()
        {
            Players.SelectedIndex = Game.PlayerIndex;
            Cities.Content = Game.CurrentPlayer.CurrentCity;
            UnitBox.ItemsSource = ArmyLoader.GetInstance().Soldiers();
            CurrentUnit.ItemsSource = Game.CurrentPlayer.CurrentCity.Army.getUnits().Where(n => !n.InConstruction);
            TabArmy.IsEnabled = Game.CurrentPlayer.CurrentCity.FinishedBuildings.Any(t => t is Casern);
            TabTrade.IsEnabled = Game.CurrentPlayer.CurrentCity.FinishedBuildings.Any(t => t is Market);
            UpdateRessource();
            UpdateTechnologyTab();
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
            Building currentBuilding = buildings.First(n => n.Name == currentValue);
            tbBuildingDatas.Text = currentBuilding.Description;
            tbBuildingDatas.Text += currentBuilding.Requirement.toString();
            tbBuildingDatas.Text += " Nombre de tours nécessaires : ";
            tbBuildingDatas.Text += BuildingLoader.GetInstance().GetBuilding(currentBuilding.ID).TurnsLeft;
        }

        private void btnNewBuilding_Click(object sender, RoutedEventArgs e)
        {
            if (Game.CurrentPlayer.CurrentCity.AddBuilding(cbSelectBuilding.SelectedIndex)!=null)
            {
                MessageBox.Show("Bâtiment créé !!!");
            }
            else 
            {
                MessageBox.Show( "Le bâtiment n'a pu être créé faute de ressources!");

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
                int armyTypeId = ((ArmyUnit) UnitBox.SelectedItem).ID;
                int quantity = SoldierQuantityBox.Value.Value;
                for (var i = 0; i < quantity; i++)
                {
                    if (!Game.CurrentPlayer.CurrentCity.AddArmy(armyTypeId))
                    {
                        MessageBox.Show("Pas assez de ressource pour en produire!", "Production échouée", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        private void UpdateTechnologyTab()
        {
            TechnologyToDo.SelectedIndex = 0;
            ApplyCountSlider.Value = 0;
            ApplyCountSlider.IsEnabled = false;
            Dictionary<int, Technology> tech = new Dictionary<int, Technology>();
            bool isDone = false;
            foreach (var each in TechnologyLoader.GetInstance().Technologies())
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
                TechnologyToDo.ItemsSource = tech.Select(n => (Technology)n.Value);
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
                UpgradableEntityList.ItemsSource = Enumerable.Empty<UpgradableEntity>();
                ApplyCountSlider.IsEnabled = true;
                ApplyCountSlider.Value = 0;
                ApplyCountSlider.Maximum = selection.Count;
            }
        }

        private void TechnologyButton_Click(object sender, RoutedEventArgs e)
        {
            
            if(technologySelectedIsResearched)
            {
                var technologyDone = (Technology)TechnologyDone.SelectedItem;
                if (technologyDone == null)
                    return;
                var entity = (CountableListItem<UpgradableEntity>)UpgradableEntityList.SelectedItem;
                if (entity != null)
                {
                    Game.CurrentPlayer.CurrentCity.UpgradeEntities(entity.Item, technologyDone, (int)ApplyCountSlider.Value);
                }
                Update();
            }
            else
            {
                var technologyToDo = (Technology)TechnologyToDo.SelectedItem;
                if (technologyToDo == null)
                    return;
                if(Game.CurrentPlayer.ResearchTechnology(technologyToDo.ID))
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

                var soldiers = Game.CurrentPlayer.CurrentCity.Army.getUnits()
                    .Where(soldier => technology.AffectedSoldiers.Any(x => soldier.ID == x))
                    .GroupBy(x => x.ID)
                    .Select(x => new CountableListItem<UpgradableEntity>(x.First(), x.Count()));

                UpgradableEntityList.ItemsSource = buildings.Union(soldiers);
                technologySelectedIsResearched = true;
            }
            else
            {
                technologySelectedIsResearched = false;
                //Technology pas encore faite
                MessageBox.Show("Hey");
            }
        }

        private void UnitBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ArmyRequirementUpdate();
        }

        private void ArmyRequirementUpdate()
        {
            var army = (ArmyUnit)UnitBox.SelectedItem;
            if (army == null)
                return;
            try
            {
                int number = (int)SoldierQuantityBox.Value;
                ArmyRequirement.Content = "";
                ArmyRequirement.Content += "Or : " + army.Requirement.Resources.get("Gold") * number;
                ArmyRequirement.Content += " / Viande : " + army.Requirement.Resources.get("Meat") * number;
                ArmyRequirement.Content += " / Bois : " + army.Requirement.Resources.get("Wood") * number;
                ArmyRequirement.Content += " / Roche : " + army.Requirement.Resources.get("Rock") * number;
                ArmyRequirement.Content += " / Population : " + army.Requirement.Resources.get("Population") * number;
            }
            catch (InvalidOperationException)
            {

            }
        }

        private void SoldierQuantityBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SoldierQuantityBox.Value < 0)
                SoldierQuantityBox.Value = 0;
            ArmyRequirementUpdate();
        }

    }
}
