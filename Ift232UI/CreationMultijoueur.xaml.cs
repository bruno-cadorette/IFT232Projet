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
using Xceed.Wpf.Toolkit;
using ProjetIft232;

namespace Ift232UI
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class CreationMultijoueur : Window
    {
        public Game Game;
        public CreationMultijoueur()
        {
            InitializeComponent();
        }

        public CreationMultijoueur(Game gm)
        {
            InitializeComponent();
            Game = gm;
        }
        private void CreatePlayers_Click(object sender, RoutedEventArgs e)
        {
            int j = FirstValue.Value.GetValueOrDefault();
            CreationJoueur j1;
            while(Game.Players.Count < j)
            {
                j1 = new CreationJoueur(Game);
                j1.ShowDialog();
                j1.Close();
            }
            this.Close();
        }
    }
}
