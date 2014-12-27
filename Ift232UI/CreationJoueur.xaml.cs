using System.Windows;
using Core;

namespace Ift232UI
{
    /// <summary>
    ///     Logique d'interaction pour Window1.xaml
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
            }
            else if (Game.Players.Find(n => n.playerName == tbPlayer.Text) != null)
            {
                tbPlayer.Text = "Je copie le nom des autres";
            }
            else
            {
                Game.CreatePlayer(tbPlayer.Text, tbCity.Text);
                Close();
            }
        }
    }
}