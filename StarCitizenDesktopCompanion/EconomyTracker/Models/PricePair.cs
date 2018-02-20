using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models
{
    public class PricePair : BaseModel
    {
        private double _buyPrice = 0;
        private double _sellPrice = 0;
        
        public Commodity Commodity { get; }
        public double BuyPrice
        {
            get { return _buyPrice; }
            set
            {
                if (_buyPrice != value)
                {
                    _buyPrice = value;
                    RaisePropertyChanged("BuyPrice");
                }
            }
        }
        public double SellPrice
        {
            get { return _sellPrice; }
            set
            {
                if (_sellPrice != value)
                {
                    _sellPrice = value;
                    RaisePropertyChanged("SellPrice");
                }
            }
        }

        public PricePair(Commodity commodity, Guid guid = new Guid()) : base(guid)
        {
            this.Commodity = commodity;
        }
    }
}
