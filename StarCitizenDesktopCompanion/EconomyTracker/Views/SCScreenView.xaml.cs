using EconomyTracker.ViewModel;
using StarCitizenModelLibrary.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EconomyTracker.Views
{
    /// <summary>
    /// Interaction logic for SCTabView.xaml
    /// </summary>
    public partial class SCScreenView : UserControl
    {
        public SCMainViewModel MainViewModel
        {
            get { return (SCMainViewModel)GetValue(MainViewModelProperty); }
            set { SetValue(MainViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainViewModelProperty =
            DependencyProperty.Register("MainViewModel", typeof(SCMainViewModel), typeof(SCScreenView));

        public SCScreenView()
        {
            InitializeComponent();
        }
    }
}
