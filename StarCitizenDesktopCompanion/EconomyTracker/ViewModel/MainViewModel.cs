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
        private TradingPort _selectedPort;
        private string _newPortName = "";

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

        public ObservableCollection<Commodity> Commodities { get; }
        public ObservableCollection<TradingPort> TradingPorts { get; }
        
        public ICommand AddPortCommand { get; }

        public MainViewModel()
        {
            this.Commodities = new ObservableCollection<Commodity>();
            this.TradingPorts = new ObservableCollection<TradingPort>();
            this.AddPortCommand = new RelayCommand(addPortExecute, canAddPort);
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


    }
}