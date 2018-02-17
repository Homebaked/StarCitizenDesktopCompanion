using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models
{
    public class PricePair
    {
        private double _buyPrice = 0;
        private double _sellPrice = 0;

        public Guid Guid { get; }
        public Commodity Commodity { get; }
        public double BuyPrice
        {
            get { return _buyPrice; }
            set { _buyPrice = value; }
        }
        public double SellPrice
        {
            get { return _sellPrice; }
            set { _sellPrice = value; }
        }

        public PricePair(Commodity commodity, Guid guid = new Guid())
        {
            this.Commodity = commodity;
            this.Guid = guid;
        }
    }
}
