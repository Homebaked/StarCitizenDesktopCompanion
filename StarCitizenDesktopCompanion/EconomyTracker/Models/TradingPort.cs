using EconomyTracker.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models
{
    public class TradingPort : ILocation
    {
        public Guid Guid { get; }
        public string Name { get; }
        public List<PricePair> Goods { get; }

        public TradingPort(string name, Guid guid = new Guid())
        {
            this.Name = name;
            this.Guid = guid;
            this.Goods = new List<PricePair>();
        }
    }
}
