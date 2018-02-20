using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyTracker.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public Guid Guid { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected BaseModel(Guid guid)
        {
            this.Guid = guid;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
