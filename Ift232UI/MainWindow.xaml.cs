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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            test1 = "lol";
            InitializeComponent();
            t = new Timer();
            t.Interval = 1000;
            t.Elapsed += t_Elapsed;
            t.Start();
            Game = new Game();
            Game.Players.Add(new Player());
            
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            Random rd = new Random();
            test1 = rd.Next().ToString();
            Game.Players.Add(new Player());
        }

    }
}
