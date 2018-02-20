using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models
{
    public class Commodity : BaseModel
    {
        public string Name { get; }

        public Commodity(string name, Guid guid = new Guid()) : base(guid)
        {
            this.Name = name;
        }
    }
}
