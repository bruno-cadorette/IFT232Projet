using System.Windows;
using Microsoft.Win32;
using ProjetIft232;

namespace Ift232UI
{
    /// <summary>
    ///     Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Inscription : Window
    {
        public Game Game;

        public Inscription()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game = new Game();
            CreationJoueur j1;
            while (Game.Players.Count <= 0)
            {
                j1 = new CreationJoueur(Game);
                j1.ShowDialog();
                j1.Close();
            }
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Game = new Game();
            CreationMultijoueur mj1 = new CreationMultijoueur(Game);
            mj1.ShowDialog();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Game = UiTools.Load();
            Close();
        }

        public Game GetGame()
        {
            return Game;
        }
    }
}