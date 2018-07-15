using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenModelLibrary.Models
{
    public class RelationshipCollection<T> : ObservableCollection<T>
    {
        public object Parent { get; }

        public event Action<RelationshipChangedArgs> RelationshipChanged;

        public RelationshipCollection(object parent)
        {
            this.Parent = parent;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            RelationshipChanged.Invoke(new RelationshipChangedArgs(this.Parent, e));
        }
    }

    public class RelationshipChangedArgs
    {
        public object Parent { get; }

        public NotifyCollectionChangedEventArgs Args { get; }

        public RelationshipChangedArgs(object parent, NotifyCollectionChangedEventArgs args)
        {
            this.Parent = parent;
            this.Args = args;
        }
    }
}
