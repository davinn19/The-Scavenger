using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Populates an item buffer.
    /// </summary>
    [System.Serializable]
    public class ItemStack
    {
        [field: SerializeField] public Item Item { get; private set; }

        /// <remarks>DO NOT edit the amount directly unless you are in ItemBuffer. Use Extract/Insert functions in ItemBuffer instead.</remarks>
        [Min(0)] public int Amount;
        public Dictionary<string, string> data;


        public ItemStack(Item item, int amount, Dictionary<string, string> data = null)
        {
            SetItem(item);
            Amount = amount;

            if (data != null)
            {
                this.data = data;
            }
            else
            {
                this.data = new Dictionary<string, string>();
            }
        }

        public ItemStack(ItemStack otherStack, int newAmount) : this(otherStack.Item, newAmount, new Dictionary<string, string>(otherStack.data)) { }
        public ItemStack(ItemStack otherStack) : this(otherStack.Item, otherStack.Amount) { }

        public ItemStack() => Clear();

        /// <summary>
        /// Resets the itemStack's information.
        /// </summary>
        public void Clear()
        {
            Item = null;
            Amount = 0;
            data = new Dictionary<string, string>();
        }

        /// <summary>
        /// Assigns an itemStack an item. Fails if the itemStack is already assigned an item.
        /// </summary>
        /// <param name="newItem">The item to assign the itemStack with.</param>
        public void SetItem(Item newItem)
        {
            if (Item != null)
            {
                Debug.LogWarning("ItemStack is already assigned an item");
                return;
            }

            Item = newItem;

            if (Item == null)
            {
                Clear();
            }
        }

        /// <summary>
        /// Checks if another stack is allowed to insert items into it.
        /// </summary>
        /// <param name="other">The other itemStack to test with.</param>
        /// <returns>True if they are stackable.</returns>
        public bool IsStackable(ItemStack other)
        {
            // If current stack is empty, it is stackable
            if (Item == null)
            {
                return true;
            }

            // If stacks do not have the same item, it is unstackable
            if (Item != other.Item)
            {
                return false;
            }

            // If the stacks' data are different, it is unstackable
            if (data.Count != other.data.Count)
            {
                return false;
            }

            foreach (string key in data.Keys)
            {
                if (!other.data.ContainsKey(key) || other.data[key] != data[key])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the itemStack contains any items.
        /// </summary>
        /// <returns>True if the itemStack is empty.</returns>
        public bool IsEmpty()
        {
            return Amount <= 0;
        }

        /// <summary>
        /// An itemStack without an item is considered null. All itemStacks must be assigned an item.
        /// </summary>
        public static implicit operator bool(ItemStack itemStack)
        {
            return itemStack != null && itemStack.Item != null;
        }
    }
}
