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

namespace EconomyTracker.Grids
{
    /// <summary>
    /// Interaction logic for CommodityPriceDataGrid.xaml
    /// </summary>
    public partial class CommodityPriceDataGrid : UserControl
    {


        public IEnumerable<CommodityPrice> Prices
        {
            get { return (IEnumerable<CommodityPrice>)GetValue(PricesProperty); }
            set { SetValue(PricesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Prices.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PricesProperty =
            DependencyProperty.Register("Prices", typeof(IEnumerable<CommodityPrice>), typeof(CommodityPriceDataGrid), new PropertyMetadata(new List<CommodityPrice>()));



        public CommodityPriceDataGrid()
        {
            InitializeComponent();
        }
    }
}
