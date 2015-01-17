using System.Windows;
using Core;

namespace Ift232UI
{
    /// <summary>
    ///     Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class CreationMultijoueur : Window
    {
        public Game Game;

        public CreationMultijoueur()
        {
            InitializeComponent();
        }

        public CreationMultijoueur(Game game)
        {
            InitializeComponent();
            Game = game;
        }

        private void CreatePlayers_Click(object sender, RoutedEventArgs e)
        {
            int j = FirstValue.Value.GetValueOrDefault();
            CreationJoueur j1;
            while (Game.Players.Count < j)
            {
                j1 = new CreationJoueur(Game);
                j1.ShowDialog();
                j1.Close();
            }
            Close();
        }
    }
}