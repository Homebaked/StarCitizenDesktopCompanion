using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models
{
    public class Location
    {
        public string Name { get; }
        public List<Location> Children { get; }
    }
}
