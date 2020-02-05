using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venus.Utilities
{
    /// <summary>
    /// A limited list of the type T. Has the function TryAddItem to check if the list has free slots for it.
    /// </summary>
    /// <typeparam name="T">Type of the objects in this list.</typeparam>

    public class LimitedList<T> : List<T>
    {
        /// <summary>
        /// Max slots of this limited list. Get only.
        /// </summary>
        public int MaxSlots
        {
            get
            {
                return maxSlots;
            }
        }

        /// <summary>
        /// Max slots of this limited list.
        /// </summary>
        protected int maxSlots;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxSlots">Max slots on this limited list.</param>
        public LimitedList(int maxSlots)
        {
            this.maxSlots = maxSlots;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LimitedList() { }

        /// <summary>
        /// Sets the max slot amount to the given value.
        /// </summary>
        /// <param name="value">New max slot amount.</param>
        public void SetMaxSlots (int value)
        {
            maxSlots = value;
        }
        
        /// <summary>
        /// Adds the given item to th list if there is enough space for it. If not, then it returns false.
        /// </summary>
        /// <param name="itemToAdd">Item to try to add to the list.</param>
        /// <returns>True if addin the item was successful.</returns>
        public bool TryAddItem(T itemToAdd)
        {
            if (Count < maxSlots) //Check if the list has free slots.
            {
                Add(itemToAdd); //Add

                return true;
            }

            return false;
        }
    }
}