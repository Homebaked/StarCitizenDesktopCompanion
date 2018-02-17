using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models
{
    public class Commodity
    {
        public Guid Guid { get; }
        public string Name { get; }

        public Commodity(string name, Guid guid = new Guid())
        {
            this.Name = name;
            this.Guid = guid;
        }
    }
}
