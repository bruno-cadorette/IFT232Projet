using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjetIft232;

namespace Ift232UI
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class CreationJoueur : Window
    {
        public Game Game;
        public CreationJoueur()
        {
            InitializeComponent();
        }

        public CreationJoueur(Game gm)
        {
            InitializeComponent();
            Game = gm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbPlayer.Text == "")
            {
                tbPlayer.Text = "Homme sans nom";
                if (tbCity.Text == "")
                    tbCity.Text = "La cité sans nom";
            }
            else if (tbCity.Text == "")
            {
                tbCity.Text = "La cité sans nom";
            } else if (Game.Players.Find( n => n.playerName == tbPlayer.Text) != null)
            {
                tbPlayer.Text = "Je copie le nom des autres";
            }else
            {
                Player player = new Player();
                player.playerName = tbPlayer.Text;
                player.Cities.Add(new City(tbCity.Text));
                player.NextCity();
                Game.Players.Add(player);
                this.Close();
            }
        }
    }
}
