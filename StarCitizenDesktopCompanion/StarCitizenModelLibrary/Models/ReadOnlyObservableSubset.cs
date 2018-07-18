using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenModelLibrary.Models
{
    public class ReadOnlyObservableSubset<T> : ReadOnlyObservableCollection<T> where T : INotifyPropertyChanged
    {
        private readonly ObservableCollection<T> subsetCollection;
        private readonly Func<T, bool> condition;

        public ReadOnlyObservableSubset(ObservableCollection<T> collection, Func<T, bool> condition) : this(collection, filterCollection(collection, condition), condition) { }
        private ReadOnlyObservableSubset(ObservableCollection<T> originalCollection, ObservableCollection<T> subsetCollection, Func<T, bool> condition) : base(subsetCollection)
        {
            this.subsetCollection = subsetCollection;
            this.condition = condition;

            originalCollection.CollectionChanged += originalCollectionChanged;
            foreach(T item in originalCollection)
            {
                item.PropertyChanged += itemChanged;
            }
        }

        private void originalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(T item in e.NewItems)
                {
                    item.PropertyChanged += itemChanged;
                    if (condition(item))
                    {
                        subsetCollection.Add(item);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach(T item in e.OldItems)
                {
                    item.PropertyChanged -= itemChanged;
                    if (subsetCollection.Contains(item))
                    {
                        subsetCollection.Remove(item);
                    }
                }
            }
        }
        private void itemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is T item)
            {
                if (subsetCollection.Contains(item))
                {
                    if (!condition(item))
                    {
                        subsetCollection.Remove(item);
                    }
                }
                else
                {
                    if (condition(item))
                    {
                        subsetCollection.Add(item);
                    }
                }
            }
            else
            {
                throw new Exception("Item isn't of type T.");
            }
        }

        private static ObservableCollection<T> filterCollection(ObservableCollection<T> collection, Func<T, bool> condition)
        {
            ObservableCollection<T> filteredCollection = new ObservableCollection<T>();
            foreach(T item in collection)
            {
                if (condition(item))
                {
                    filteredCollection.Add(item);
                }
            }
            return filteredCollection;
        }
    }
}
