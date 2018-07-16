using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenModelLibrary.Models
{
    public class Commodity : BaseModel
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

        public Commodity(string name) : this(name, Guid.NewGuid()) { }
        public Commodity(string name, Guid guid) : base(guid)
        {
            this.Name = name;
        }
    }
}
