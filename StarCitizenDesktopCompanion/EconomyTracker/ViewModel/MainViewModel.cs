using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using StarCitizenModelLibrary.Models;
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
        private TradingPort _selectedTradingPort;

        public SCDataManager DataManager { get; } = new SCDataManager();
        public SCDeltaManager DeltaManager { get; }

        public string NewCommodityName { get; set; } = "";
        public string NewTradingPortName { get; set; } = "";

        public RelayCommand<string> AddCommodityCommand { get; }
        public RelayCommand<string> AddTradingPortCommand { get; }

        public TradingPort SelectedTradingPort
        {
            get { return _selectedTradingPort; }
            set
            {
                if (_selectedTradingPort != value)
                {
                    _selectedTradingPort = value;
                    RaisePropertyChanged("SelectedTradingPort");
                }
            }
        }

        public MainViewModel()
        {
            this.DeltaManager = new SCDeltaManager(this.DataManager);

            this.AddCommodityCommand = new RelayCommand<string>(addCommodityExecute);
            this.AddTradingPortCommand = new RelayCommand<string>(addTradingPortExecute);
        }

        private void addCommodityExecute(string name)
        {
            this.DataManager.Commodities.Add(new Commodity(name));
        }
        private void addTradingPortExecute(string name)
        {
            this.DataManager.TradingPorts.Add(new TradingPort(name));
        }
    }
}