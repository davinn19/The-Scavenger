using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
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

        public ItemStack(ItemStack otherStack) : this(otherStack.Item, otherStack.Amount, new Dictionary<string, string>(otherStack.data)) { }

        public ItemStack() => Clear();

        public void Clear()
        {
            Item = null;
            Amount = 0;
            data = new Dictionary<string, string>();
        }


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

        public bool IsEmpty()
        {
            return Amount <= 0;
        }

        public static implicit operator bool(ItemStack itemStack)
        {
            return itemStack != null && itemStack.Item != null;
        }
    }
}
