using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenModelLibrary.Models
{
    public class SCDataManager : BaseModel
    {
        public ObservableCollection<Commodity> Commodities { get; }
        public ObservableCollection<TradingPort> TradingPorts { get; }

        public SCDataManager(Guid guid = new Guid()) : base(guid)
        {
            this.Commodities = new ObservableCollection<Commodity>();
            this.TradingPorts = new ObservableCollection<TradingPort>();
        }
    }
}
