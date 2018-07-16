using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using StarCitizenDatabaseInterfacer;
using StarCitizenModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private string filePath = string.Format("{0}//companionFile.scdb", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        private SCDataManager _dataManager = new SCDataManager();

        private string _newCommodityName = "";
        private string _newTradingPortName = "";

        public SCDataManager DataManager
        {
            get { return _dataManager; }
            private set
            {
                if (_dataManager != value)
                {
                    _dataManager = value;
                    RaisePropertyChanged("DataManager");
                }
            }
        }
        public SCDeltaManager DeltaManager { get; }

        public TradingPortsViewModel PortsVM { get; }

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
        public string NewTradingPortName
        {
            get { return _newTradingPortName; }
            set
            {
                if (_newTradingPortName != value)
                {
                    _newTradingPortName = value;
                    RaisePropertyChanged("NewTradingPortName");
                }
            }
        }

        public RelayCommand SaveAsCommand { get; }
        public RelayCommand LoadCommand { get; }

        public RelayCommand<string> AddCommodityCommand { get; }
        public RelayCommand<string> AddTradingPortCommand { get; }

        public MainViewModel()
        {
            this.DeltaManager = new SCDeltaManager(this.DataManager);

            this.PortsVM = new TradingPortsViewModel(this.DataManager);

            this.SaveAsCommand = new RelayCommand(saveAsExecute);
            this.LoadCommand = new RelayCommand(loadExecute);

            this.AddCommodityCommand = new RelayCommand<string>(addCommodityExecute);
            this.AddTradingPortCommand = new RelayCommand<string>(addTradingPortExecute);
        }

        private void saveAsExecute()
        {
            if (File.Exists(this.filePath))
            {
                File.Delete(this.filePath);
            }
            StarCitizenDB.SaveNew(this.filePath, this.DataManager);
        }
        private void loadExecute()
        {
            this.DataManager = StarCitizenDB.Load(filePath);
        }

        private void addCommodityExecute(string name)
        {
            this.DataManager.Commodities.Add(new Commodity(name));
            this.NewCommodityName = "";
        }
        private void addTradingPortExecute(string name)
        {
            this.DataManager.TradingPorts.Add(new TradingPort(name));
            this.NewTradingPortName = "";
        }
    }
}