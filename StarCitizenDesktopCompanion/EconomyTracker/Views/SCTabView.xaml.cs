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
    public partial class SCTabView : UserControl
    {


        public SCDataManager DataManager
        {
            get { return (SCDataManager)GetValue(DataManagerProperty); }
            set { SetValue(DataManagerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataManager.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataManagerProperty =
            DependencyProperty.Register("DataManager", typeof(SCDataManager), typeof(SCTabView));



        public SCTabView()
        {
            InitializeComponent();
        }
    }
}
