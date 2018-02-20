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
            this.AddCommodityCommand = new RelayCommand(addCommodityExecute, canAddCommodity);
        }

        private void addPortExecute()
        {
            TradingPorts.Add(new TradingPort(this.NewPortName));
            this.NewPortName = "";
        }
        private bool canAddPort()
        {
            return this.NewPortName != "";
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