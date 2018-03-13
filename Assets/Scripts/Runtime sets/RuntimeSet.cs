using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime_sets
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        public IList<T> Items = new List<T>();

        /// <summary>
        /// Adds the item to the list if it isn't already in it
        /// </summary>
        /// <param name="item"></param>
        public void AddNonDup( T item )
        {
            if (!Items.Contains(item))
            {
                Items.Add(item);
            }
        }

        /// <summary>
        /// Removes an item from the list
        /// </summary>
        /// <param name="item"></param>
        public void Remove( T item )
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
            }
        }

    }
}
