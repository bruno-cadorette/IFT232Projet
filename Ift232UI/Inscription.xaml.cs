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
    public partial class Inscription : Window
    {
        public Game Game;
        public Inscription()
        {
            InitializeComponent();
        }

        public Inscription(Game gm)
        {
            InitializeComponent();
            Game = gm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreationJoueur j1;
            while (Game.Players.Count <= 0)
            {
                j1 = new CreationJoueur(Game);
                j1.ShowDialog();
                j1.Close();
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreationMultijoueur mj1 = new CreationMultijoueur(Game);
            mj1.ShowDialog();
            this.Close();
        }
    }
}
