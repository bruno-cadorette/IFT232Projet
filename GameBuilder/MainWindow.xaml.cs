using Core.Buildings;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EntityCreationViewModel entityCreationViewModel;
        public MainWindow()
        {

            InitializeComponent();
            entityCreationViewModel = new EntityCreationViewModel();
            this.DataContext = entityCreationViewModel;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            entityCreationViewModel.NewCommand.Execute(null);
        }

        private void BuildingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            entityCreationViewModel.SelectCommand.Execute((sender as ListBox).SelectedItem);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            entityCreationViewModel.SaveCommand.Execute(BuildingsListBox.SelectedItem);
            BuildingsListBox.Items.Refresh();

        }
    }
}
