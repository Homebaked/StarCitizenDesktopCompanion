using EconomyTracker.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models
{
    public class TradingPort : BaseModel, ILocation
    {
        public string Name { get; }
        public ObservableCollection<PricePair> Goods { get; }

        public TradingPort(string name, Guid guid = new Guid()) : base(guid)
        {
            this.Name = name;
            this.Goods = new ObservableCollection<PricePair>();
        }
    }
}
