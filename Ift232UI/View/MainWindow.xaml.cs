using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Core;
using Core.Military;
using Core.Buildings;
using Core.Technologies;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using Core.Map;
using System.Windows.Media;
using GameHelper;

namespace Ift232UI
{
    /// <summary>
    ///     Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game Game;
        public City City { get; set; }
        public CityManagementViewModel CityManagementViewModel { get; set; }
        public BuildingManagementViewModel BuildingManagementViewModel
        {
            get
            {
                return CityManagementViewModel.BuildingManagementViewModel;
            }
        }
        public ArmyManagementViewModel ArmyManagementViewModel
        {
            get
            {
                return CityManagementViewModel.ArmyManagementViewModel;
            }
        }
        private bool boughtResourcesIsLoaded;
        private bool soldResourcesIsLoaded;
        private bool technologySelectedIsResearched;


        public MainWindow(City city)
        {
            City = city;
            CityManagementViewModel = new CityManagementViewModel(City);
            DataContext = this;
            InitializeComponent();
            Game = new Game();
            Update();
        }

        private void Update()
        {
            UpdateTechnologyTab();
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
                //BoughtResources.SelectedValue = listeEchange.FirstOrDefault().ToString();
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
            //var boughtType = Resource.Name.First(n => n.Value == BoughtResources.SelectedItem.ToString());

   //         var soldType = Resource.Name.First(n => n.Value == SoldResources.SelectedItem.ToString());

           // int val2 = m.Trade(val1, soldType.Key, boughtType.Key);
          //  SecondValue.Text = val2.ToString(CultureInfo.InvariantCulture);

         //  FirstValue.Maximum = City.Ressources[soldType.Key];
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

            //var soldType = Resource.Name.First(n => n.Value == SoldResources.SelectedItem.ToString());
            //var boughtType = Resource.Name.First(n => n.Value == BoughtResources.SelectedItem.ToString());

            //int qty = FirstValue.Value.Value;

            //bool done = ((Market)Game.GetMarket()).Achat(City, qty, soldType.Key,
            //    boughtType.Key);
            //if (done)
            //{
            //    MessageBox.Show("Votre échange a bien eu lieu !!!");
            //}
            //else
            //{
            //    MessageBox.Show("Votre échange n'a pas eu lieu !!!");
            //}


            Update();
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
            /*foreach (var each in TechnologyFactory.GetInstance().Technologies())
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
            TechnologyDone.ItemsSource = Game.CurrentPlayer.ResearchedTech;*/
        }


        private void TechnologyToDo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var technology = (Technology)TechnologyToDo.SelectedItem;
            if (technology == null)
            {
                return;
            }
            tbDescription.Text = technology.Description;
            tbRequisite.Text = technology.Requirement.ToString();
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
                    City.UpgradeEntities(entity.Item, technologyDone,
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
            tbRequisite.Text = technology.Requirement.ToString();
            if (technology.InConstruction)
                tbRequisite.Text += " Nombre de tours restants :" + technology.TurnsLeft;
            TechnologyButton.Content = "Appliquer";

            if (Game.CurrentPlayer.ResearchedTech.Any(x => !x.InConstruction && x.ID == technology.ID))
            {
                //Technology déjà faite
                ApplyTechGrid.Visibility = Visibility.Visible;
                var buildings = City.Buildings
                    .Where(building => building.CanBeAffected(technology))
                    .GroupBy(x => x.ID)
                    .Select(x => new CountableListItem<UpgradableEntity>(x.First(), x.Count()));
                /*
                var soldiers = city.Army
                    .Where(soldier => soldier.Type.CanBeAffected(technology))
                    .GroupBy(x => x.Type.ID)
                    .Select(x => new CountableListItem<UpgradableEntity>(x.First(), x.Count()));*/
                //Quand les ID vont être unique
                var entities = City.Buildings.Cast<UpgradableEntity>()
                    .Concat(City.Army.Select(x => x.Type))
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


        private void ArmyRequirementUpdate()
        {
            Soldier army = null;
            int number = 0;
            ArmyRequirement.Content = "";
            ArmyRequirement.Content += "Or : " + army.Requirement.Resources[ResourcesType.Gold] * number;
            ArmyRequirement.Content += " / Viande : " + army.Requirement.Resources[ResourcesType.Meat] * number;
            ArmyRequirement.Content += " / Bois : " + army.Requirement.Resources[ResourcesType.Wood] * number;
            ArmyRequirement.Content += " / Roche : " + army.Requirement.Resources[ResourcesType.Rock] * number;
            ArmyRequirement.Content += " / Population : " +
                                       army.Requirement.Resources[ResourcesType.Population] * number;


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