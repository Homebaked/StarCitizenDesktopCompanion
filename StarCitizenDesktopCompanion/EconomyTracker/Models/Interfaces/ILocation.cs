using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models.Interfaces
{
    public interface ILocation
    {
        Guid Guid { get; }
        string Name { get; }
        List<TradingPort> Children { get; }
    }
}
