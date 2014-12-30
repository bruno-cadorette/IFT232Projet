using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GameBuilder
{
    /// <summary>
    /// Interaction logic for MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Window
    {
        public ObservableCollection<Button> Landscape { get; set; }
        public MapEditorViewModel mapEditorViewModel { get; set; }
        public MapEditor()
        {
            
            mapEditorViewModel = new MapEditorViewModel();
            DataContext = mapEditorViewModel;
            InitializeComponent();
            
        }
        
    }
}
