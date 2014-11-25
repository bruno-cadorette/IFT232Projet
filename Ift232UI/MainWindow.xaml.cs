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


    }
}
