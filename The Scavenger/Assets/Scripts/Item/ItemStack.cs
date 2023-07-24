using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [System.Serializable]
    public class ItemStack
    {
        [field: SerializeField] public Item Item { get; private set; }
        [Min(0)] public int amount;
        public Dictionary<string, string> data;


        public ItemStack(Item item, int amount, Dictionary<string, string> data = null)
        {
            SetItem(item);
            this.amount = amount;

            if (data != null)
            {
                this.data = data;
            }
            else
            {
                this.data = new Dictionary<string, string>();
            }
        }

        public ItemStack(ItemStack otherStack) : this(otherStack.Item, otherStack.amount, new Dictionary<string, string>(otherStack.data))
        {

        }

        public ItemStack() => Clear();

        public void Clear()
        {
            Item = null;
            amount = 0;
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
            return amount <= 0;
        }

        public static implicit operator bool(ItemStack itemStack)
        {
            return itemStack != null && itemStack.Item != null;
        }
    }
}
