using StarCitizenModelLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenModelLibrary.Models
{
    public class TradingPort : BaseModel, ILocation
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public ObservableCollection<PricePair> Goods { get; }

        public TradingPort(string name, Guid guid = new Guid()) : base(guid)
        {
            this.Name = name;
            this.Goods = new ObservableCollection<PricePair>();
        }
    }
}
