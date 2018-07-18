using GalaSoft.MvvmLight;
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
        private ReadOnlyObservableCollection<CommodityPrice> _selectedBuyPrices;
        private ReadOnlyObservableCollection<CommodityPrice> _selectedSellPrices;
        
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
                }
            }
        }
        public ReadOnlyObservableCollection<CommodityPrice> SelectedBuyPrices
        {
            get { return _selectedBuyPrices; }
            private set
            {
                if (_selectedBuyPrices != value)
                {
                    _selectedBuyPrices = value;
                    RaisePropertyChanged("SelectedBuyPrices");
                }
            }
        }
        public ReadOnlyObservableCollection<CommodityPrice> SelectedSellPrices
        {
            get { return _selectedBuyPrices; }
            private set
            {
                if (_selectedSellPrices != value)
                {
                    _selectedSellPrices = value;
                    RaisePropertyChanged("SelectedSellPrices");
                }
            }
        }

        public TradingPortsViewModel(SCDataManager dataManager)
        {
            this.Ports = new ReadOnlyObservableCollection<TradingPort>(dataManager.TradingPorts);
            this.SelectedPort.PropertyChanged += selectedPortPropertyChanged;
        }

        private void selectedPortPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
