using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using StarCitizenModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.ViewModel
{
    public class CommodityPricesViewModel : ViewModelBase
    {
        public PriceType Type { get; }

        private TradingPort _port;
        private ReadOnlyObservableSubset<CommodityPrice> _prices;
        private Commodity _selectedCommodity;
        private double _newPrice;

        public TradingPort Port
        {
            get { return _port; }
            set
            {
                if (_port != value)
                {
                    _port = value;
                    RaisePropertyChanged("Port");
                    portChanged(value);
                }
            }
        }
        public ReadOnlyObservableSubset<CommodityPrice> Prices
        {
            get { return _prices; }
            private set
            {
                if (_prices != value)
                {
                    _prices = value;
                    RaisePropertyChanged("Prices");
                }
            }
        }
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

        public CommodityPricesViewModel(PriceType type)
        {
            this.Type = type;
        }

        private void portChanged(TradingPort port)
        {
            this.Prices = new ReadOnlyObservableSubset<CommodityPrice>(this.Port.Prices, (x) => { return x.Type == this.Type; });
        }

        private void addPriceExecute()
        {
            bool commodityFound = false;
            foreach(CommodityPrice price in this.Port.Prices)
            {
                if (price.Commodity == this.SelectedCommodity)
                {
                    price.AddPrice(this.NewPrice);
                    commodityFound = true;
                }
            }
            if (!commodityFound)
            {
                CommodityPrice newCommodityPrice = new CommodityPrice(this.SelectedCommodity, this.Type);
                newCommodityPrice.AddPrice(this.NewPrice);
                this.Port.Prices.Add(newCommodityPrice);
            }

            this.SelectedCommodity = null;
            this.NewPrice = 0;
        }
    }
}
