using StarCitizenModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenModelLibrary.Interfaces
{
    public interface ILocation
    {
        Guid Guid { get; }
        string Name { get; }
    }
}
