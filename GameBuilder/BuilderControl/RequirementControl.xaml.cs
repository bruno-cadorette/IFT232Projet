using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameBuilder.BuilderControl
{
    /// <summary>
    /// Interaction logic for RequirementSelector.xaml
    /// </summary>
    public partial class RequirementControl : UserControl
    {
        //AddCommand="{Binding AddToRequirementCommand}" SelectorItemSource="{Binding Buildings}" ListItemSource="{Binding CurrentBuilding.Requirements}"
        public static readonly DependencyProperty SelectorProperty = DependencyProperty.Register("SelectorItemSource", typeof(IEnumerable), typeof(RequirementControl));
        public static readonly DependencyProperty ListProperty = DependencyProperty.Register("ListItemSource", typeof(IEnumerable), typeof(RequirementControl));
        public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(RequirementControl));

        public IEnumerable SelectorItemSource
        {
            get
            {
                return (IEnumerable)GetValue(SelectorProperty);
            }
            set
            {
                SetValue(SelectorProperty, value);
            }
        }
        public IEnumerable ListItemSource
        {
            get
            {
                return (IEnumerable)GetValue(ListProperty);
            }
            set
            {
                SetValue(ListProperty, value);
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return (ICommand)GetValue(AddCommandProperty);
            }
            set
            {
                SetValue(AddCommandProperty, value);
            }
        }

        public string Header
        {
            get
            {
                return RequirementGroupBox.Header.ToString();
            }
            set
            {
                RequirementGroupBox.Header = value;
            }
        }

        public RequirementControl()
        {
            InitializeComponent();
        }
    }
}
