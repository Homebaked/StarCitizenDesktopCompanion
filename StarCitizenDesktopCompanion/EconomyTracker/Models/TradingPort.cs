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
        public List<TradingPort> Children
        {
            get { return new List<TradingPort>(); }
        }
    }
}
