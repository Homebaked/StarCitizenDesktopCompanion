using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using StarCitizenModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.ViewModel
{
    public class TradingPortsViewModel : ViewModelBase
    {
        private TradingPort _selectedPort;
        private ReadOnlyObservableSubset<CommodityPrice> _selectedBuyPrices;
        private ReadOnlyObservableSubset<CommodityPrice> _selectedSellPrices;
        
        public ReadOnlyObservableCollection<TradingPort> Ports { get; }
        public TradingPort SelectedPort
        {
            get { return _selectedPort; }
            set
            {
                if (_selectedPort != value)
                {
                    _selectedPort = value;
                    RaisePropertyChanged("SelectedPort");
                    selectedPortChanged(value);
                }
            }
        }

        public CommodityPricesViewModel BuyPrices { get; } = new CommodityPricesViewModel(PriceType.Buy);
        public CommodityPricesViewModel SellPrices { get; } = new CommodityPricesViewModel(PriceType.Sell);

        public TradingPortsViewModel(SCDataManager dataManager)
        {
            this.Ports = new ReadOnlyObservableCollection<TradingPort>(dataManager.TradingPorts);
        }

        private void selectedPortChanged(TradingPort port)
        {
            this.BuyPrices.Port = port;
            this.SellPrices.Port = port;
        }
    }
}
