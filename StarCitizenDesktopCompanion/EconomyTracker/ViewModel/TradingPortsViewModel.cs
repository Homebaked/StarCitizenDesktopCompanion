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
        private Commodity _selectedCommodity;
        private PriceType _selectedType;
        private double _newPrice;

        private ReadOnlyObservableSubset<CommodityPrice> _buyPrices;
        private ReadOnlyObservableSubset<CommodityPrice> _sellPrices;
        
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

        public ReadOnlyObservableSubset<CommodityPrice> BuyPrices
        {
            get { return _buyPrices; }
            private set
            {
                if (_buyPrices != value)
                {
                    _buyPrices = value;
                    RaisePropertyChanged("BuyPrices");
                }
            }
        }
        public ReadOnlyObservableSubset<CommodityPrice> SellPrices
        {
            get { return _sellPrices; }
            private set
            {
                if (_sellPrices != value)
                {
                    _sellPrices = value;
                    RaisePropertyChanged("SellPrices");
                }
            }
        }

        public ReadOnlyObservableCollection<Commodity> Commodities { get; }
        public Commodity SelectedCommodity
        {
            get { return _selectedCommodity; }
            set
            {
                if (_selectedCommodity != value)
                {
                    _selectedCommodity = value;
                    RaisePropertyChanged("SelectedCommodity");
                }
            }
        }

        public List<PriceType> Types { get; } = new List<PriceType>() { PriceType.Buy, PriceType.Sell };
        public PriceType SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    RaisePropertyChanged("SelectedType");
                }
            }
        }

        public double NewPrice
        {
            get { return _newPrice; }
            set
            {
                if (_newPrice != value)
                {
                    _newPrice = value;
                    RaisePropertyChanged("NewPrice");
                }
            }
        }

        public RelayCommand AddPriceCommand { get; }

        public TradingPortsViewModel(SCDataManager dataManager)
        {
            this.Ports = new ReadOnlyObservableCollection<TradingPort>(dataManager.TradingPorts);
            this.Commodities = new ReadOnlyObservableCollection<Commodity>(dataManager.Commodities);

            this.AddPriceCommand = new RelayCommand(addPriceExecute, canAddPrice);
        }

        private void selectedPortChanged(TradingPort port)
        {
            this.BuyPrices = new ReadOnlyObservableSubset<CommodityPrice>(port.Prices, price => price.Type == PriceType.Buy);
            this.SellPrices = new ReadOnlyObservableSubset<CommodityPrice>(port.Prices, price => price.Type == PriceType.Sell);
        }

        private void addPriceExecute()
        {
            CommodityPrice matchingPrice = null;
            foreach(CommodityPrice price in this.SelectedPort.Prices)
            {
                if (price.Commodity == this.SelectedCommodity && price.Type == this.SelectedType)
                {
                    matchingPrice = price;
                    break;
                }
            }

            if (matchingPrice == null)
            {
                matchingPrice = new CommodityPrice(this.SelectedCommodity, this.SelectedType);
            }

            matchingPrice.AddPrice(this.NewPrice);

            this.SelectedCommodity = null;
            this.NewPrice = 0;
        }
        private bool canAddPrice()
        {
            return (this.SelectedPort != null &&
                this.SelectedCommodity != null &&
                this.NewPrice > 0 &&
                this.SelectedType != 0);
        }
    }
}
