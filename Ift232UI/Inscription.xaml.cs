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
using Microsoft.Win32;

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
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Game = new Game();
            CreationMultijoueur mj1 = new CreationMultijoueur(Game);
            mj1.ShowDialog();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog window = new OpenFileDialog();
            window.Filter = "save file|*.sav";
            window.Title = "Séléctionnez le fichier de chargement.";
            window.CheckFileExists = true;
            if (true == window.ShowDialog())
            {
                Game=Game.Load(window.FileName);
                this.Close();
            }
        }

        public Game GetGame()
        {
            return Game;
        }
    }
}
