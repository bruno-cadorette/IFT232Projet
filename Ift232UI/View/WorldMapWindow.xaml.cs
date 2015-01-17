using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ift232UI
{
    /// <summary>
    /// Interaction logic for WorldMapWindow.xaml
    /// </summary>
    public partial class WorldMapWindow : Window
    {
        public MapViewModel MapViewModel { get; set; }
        public WorldMapWindow()
        {
            InitializeComponent();
            Inscription inscription = new Inscription();
            inscription.ShowDialog();
            MapViewModel = new MapViewModel(inscription.GetGame(), city => new MainWindow(city).ShowDialog());
            this.DataContext = MapViewModel;
        }
    }
}
