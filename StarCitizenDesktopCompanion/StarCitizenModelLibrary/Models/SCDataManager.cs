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
        public RelationshipCollection<Commodity> Commodities { get; }
        public RelationshipCollection<TradingPort> TradingPorts { get; }

        public SCDataManager() : base(Guid.NewGuid())
        {
            this.Commodities = new RelationshipCollection<Commodity>(this);
            this.TradingPorts = new RelationshipCollection<TradingPort>(this);
        }
    }
}
