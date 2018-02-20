using EconomyTracker.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EconomyTracker.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private TradingPort _selectedPort = null;
        private string _newPortName = "";
        private Commodity _newCommodity = null;
        private double _newBuyPrice = 0;
        private double _newSellPrice = 0;
        private string _newCommodityName = "";

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
        public string NewPortName
        {
            get { return _newPortName; }
            set
            {
                if (_newPortName != value)
                {
                    _newPortName = value;
                    RaisePropertyChanged("NewPortName");
                }
            }
        }
        public Commodity NewCommodity
        {
            get { return _newCommodity; }
            set
            {
                if (_newCommodity != value)
                {
                    _newCommodity = value;
                    RaisePropertyChanged("NewCommodity");
                }
            }
        }
        public double NewBuyPrice
        {
            get { return _newBuyPrice; }
            set
            {
                if (_newBuyPrice != value)
                {
                    _newBuyPrice = value;
                    RaisePropertyChanged("NewBuyPrice");
                }
            }
        }
        public double NewSellPrice
        {
            get { return _newSellPrice; }
            set
            {
                if (_newSellPrice != value)
                {
                    _newSellPrice = value;
                    RaisePropertyChanged("NewSellPrice");
                }
            }
        }
        public string NewCommodityName
        {
            get { return _newCommodityName; }
            set
            {
                if (_newCommodityName != value)
                {
                    _newCommodityName = value;
                    RaisePropertyChanged("NewCommodityName");
                }
            }
        }

        public ObservableCollection<Commodity> Commodities { get; }
        public ObservableCollection<TradingPort> TradingPorts { get; }
        
        public ICommand AddPortCommand { get; }
        public ICommand AddPricePairCommand { get; }
        public ICommand AddCommodityCommand { get; }

        public MainViewModel()
        {
            this.Commodities = new ObservableCollection<Commodity>();
            this.TradingPorts = new ObservableCollection<TradingPort>();
            this.AddPortCommand = new RelayCommand(addPortExecute, canAddPort);
            this.AddPricePairCommand = new RelayCommand(addPricePairExecute, canAddPricePair);
            this.AddCommodityCommand = new RelayCommand(addCommodityExecute, canAddCommodity);
        }

        private void addPortExecute()
        {
            this.TradingPorts.Add(new TradingPort(this.NewPortName));
            this.NewPortName = "";
        }
        private bool canAddPort()
        {
            return this.NewPortName != "";
        }

        private void addPricePairExecute()
        {
            PricePair newPP = new PricePair(NewCommodity);
            newPP.BuyPrice = NewBuyPrice;
            newPP.SellPrice = NewSellPrice;
            this.SelectedPort.Goods.Add(newPP);
            this.NewCommodity = null;
            this.NewBuyPrice = 0;
            this.NewSellPrice = 0;
            RaisePropertyChanged("SelectedPort");
        }
        private bool canAddPricePair()
        {
            return this.SelectedPort != null && this.NewCommodity != null;
        }

        private void addCommodityExecute()
        {
            this.Commodities.Add(new Commodity(NewCommodityName));
            this.NewCommodityName = "";
        }
        private bool canAddCommodity()
        {
            return this.NewCommodityName != "";
        }
    }
}