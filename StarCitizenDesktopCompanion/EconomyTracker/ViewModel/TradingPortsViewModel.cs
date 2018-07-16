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
        public ReadOnlyObservableCollection<TradingPort> Ports { get; }

        public TradingPortsViewModel(SCDataManager dataManager)
        {
            this.Ports = new ReadOnlyObservableCollection<TradingPort>(dataManager.TradingPorts);
        }
    }
}
