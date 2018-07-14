using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenModelLibrary.Models
{
    public class SCDeltaManager
    {
        private readonly SCDataManager dataManager;

        public List<DataDelta> Deltas = new List<DataDelta>();

        public SCDeltaManager(SCDataManager dataManager)
        {
            this.dataManager = dataManager;
            subscribeToDataManager(dataManager);
        }

        #region Subscribers
        private void subscribeToDataManager(SCDataManager dataManager)
        {
            dataManager.PropertyChanged += propertyChanged;
            dataManager.Commodities.CollectionChanged += collectionChanged;
            dataManager.TradingPorts.CollectionChanged += collectionChanged;

            foreach(Commodity commodity in dataManager.Commodities)
            {
                subscribeToCommodity(commodity);
            }

            foreach(TradingPort tradingPort in dataManager.TradingPorts)
            {
                subscribeToTradingPort(tradingPort);
            }
        }
        private void subscribeToCommodity(Commodity commodity)
        {
            commodity.PropertyChanged += propertyChanged;
        }
        private void subscribeToTradingPort(TradingPort tradingPort)
        {
            tradingPort.PropertyChanged += propertyChanged;
            tradingPort.Prices.CollectionChanged += collectionChanged;
            
            foreach(CommodityPrice price in tradingPort.Prices)
            {
                subscribeToCommodityPrice(price);
            }
        }
        private void subscribeToCommodityPrice(CommodityPrice price)
        {
            price.PriceHistory.CollectionChanged += collectionChanged;
        }
        #endregion

        #region Event Handlers
        private void propertyChanged(object sender, PropertyChangedEventArgs args)
        {
            this.Deltas.Add(new DataDelta(sender, args));
        }
        private void collectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.Deltas.Add(new DataDelta(sender, args));
            
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(object item in args.NewItems)
                {
                    if (item is Commodity commodity)
                    {
                        subscribeToCommodity(commodity);
                    }
                    else if (item is TradingPort tradingPort)
                    {
                        subscribeToTradingPort(tradingPort);
                    }
                    else if (item is CommodityPrice price)
                    {
                        subscribeToCommodityPrice(price);
                    }
                }
            }
        }
        #endregion
    }

    public class DataDelta
    {
        public object Sender { get; }
        public PropertyChangedEventArgs PropertyChangedArgs { get; } = null;
        public NotifyCollectionChangedEventArgs CollectionChangedArgs { get; } = null;

        public DataDelta(object sender, PropertyChangedEventArgs propertyChangedArgs)
        {
            this.Sender = sender;
            this.PropertyChangedArgs = propertyChangedArgs;
        }
        public DataDelta(object sender, NotifyCollectionChangedEventArgs collectionChangedArgs)
        {
            this.Sender = sender;
            this.CollectionChangedArgs = collectionChangedArgs;
        }
    }
}
