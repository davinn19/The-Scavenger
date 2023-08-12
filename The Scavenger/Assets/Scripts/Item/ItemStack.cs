using Leguar.TotalJSON;
using System;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Populates an item buffer.
    /// </summary>
    [System.Serializable]
    public class ItemStack
    {
        public event Action Changed;

        // Item things
        [SerializeField] private string itemID = "";
        public Item Item => Database.Instance.Item.Get(itemID);

        // Max amount things
        private int maxAmount = 100;
        public int MaxAmount => maxAmount;
        public void SetMaxAmount(int maxAmount)
        {
            Debug.Assert(maxAmount > 0);
            Debug.Assert(maxAmount == 100); // Can only be set once
            this.maxAmount = maxAmount;
            SetAmount(amount);
        }

        // Amount things
        [SerializeField, Min(0)] private int amount = 0;
        public int Amount => amount;
        public void AddAmount(int addBy) => SetAmount(amount + addBy);
        public void RemoveAmount(int removeBy) => SetAmount(amount - removeBy);
        public void SetAmount(int amount)
        {
            this.amount = Mathf.Clamp(amount, 0, maxAmount);
            if (IsEmpty())
            {
                Clear();
            }
            else
            {
                Changed?.Invoke();
            }
        }

        public int GetAmountBeforeFull() => MaxAmount - Amount;
        

        [SerializeField] private JSON persistentData = new JSON();
        public JSON PersistentData => persistentData;
        public void SetPersistentData(JSON data)
        {
            persistentData = JSONHelper.Copy(data);
            persistentData.SetProtected();
            Changed?.Invoke();
        }

        public ItemStack()
        {
            itemID = "";
            amount = 0;
            persistentData = new JSON();
            persistentData.SetProtected();
        }

        // TODO add docs
        public ItemStack(Item item, int amount, JSON persistentData)
        {
            this.amount = amount;
            this.persistentData = JSONHelper.GetJSONOrEmpty(persistentData); // TODO convert to normal???
            persistentData.SetProtected();
            itemID = item.name;
        }

        /// <summary>
        /// Copies the information of another itemStack. Will not copy max amount.
        /// </summary>
        /// <param name="copyAmount">If true, copy the itemStack's amount.</param>
        /// <param name="copyPersistentData">If true, copy the persistent data.</param>
        public void Copy(ItemStack itemStackToCopy, bool copyAmount = true, bool copyPersistentData = true)
        {
            itemID = itemStackToCopy.itemID;
            if (copyAmount)
            {
                amount = Mathf.Clamp(itemStackToCopy.amount, 0, maxAmount);
            }
            if (copyPersistentData)
            {
                persistentData = JSONHelper.Copy(itemStackToCopy.persistentData);
                persistentData.SetProtected();
            }
            Changed?.Invoke();
        }

        /// <summary>
        /// Creates a copy of another itemStack. Max amount is copies as well.
        /// </summary>
        /// <param name="amount">Set the copy's amount to something different.</param>
        /// <param name="keepPersistentData">If true, copy the persistent data.</param>
        /// <returns>The itemStack copy.</returns>
        public ItemStack Clone(int amount = -1, bool keepPersistentData = true)
        {
            if (amount < 0)
            {
                amount = this.amount;
            }

            JSON persistentData = new JSON();
            if (keepPersistentData)
            {
                JSONHelper.Copy(JSONHelper.GetJSONOrEmpty(this.persistentData));
            }

            ItemStack copy = new ItemStack(Item, amount, persistentData);
            copy.maxAmount = amount;
            return copy;
        }

        // TODO add docs
        public void Clear()
        {
            itemID = "";
            amount = 0;
            persistentData = new JSON();
            persistentData.SetProtected();
            Changed?.Invoke();
        }

        /// <summary>
        /// Checks if another stack is allowed to insert items into it.
        /// </summary>
        /// <param name="other">The other itemStack to test with.</param>
        /// <returns>True if they are stackable.</returns>
        public bool IsStackable(ItemStack other)
        {
            // If current stack is empty, it is stackable
            if (!this)
            {
                return true;
            }

            // If stacks do not have the same item, it is unstackable
            if (!SharesItem(other))
            {
                return false;
            }

            // Stacks need to both have/not have persistent data
            if (HasPersistentData() != other.HasPersistentData())
            {
                return false;
            }

            // If the stacks' data are different, it is unstackable
            if (HasPersistentData() && !persistentData.Equals(other.persistentData))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the itemStack contains persistent data.
        /// </summary>
        /// <returns>True if the itemStack contains persistent data.</returns>
        public bool HasPersistentData()
        {
            return persistentData != null && persistentData.Count > 0;
        }

        /// <summary>
        /// Checks if the two itemStacks consist of the same item, ignoring persistent data.
        /// </summary>
        /// <param name="other">ItemStack to compare to.</param>
        /// <returns>True if the itemStacks share the same item.</returns>
        public bool SharesItem(ItemStack other)
        {
            return other.Item == Item;
        }

        /// <summary>
        /// Checks if the itemStack contains any items.
        /// </summary>
        /// <returns>True if the itemStack is empty.</returns>
        public bool IsEmpty()
        {
            return amount == 0;
        }

        /// <summary>
        /// Checks if the itemStack is full.
        /// </summary>
        /// <returns>True if the itemStack is full.</returns>
        public bool IsFull()
        {
            return amount == MaxAmount;
        }

        /// <summary>
        /// An itemStack without an item is considered null.
        /// </summary>
        public static implicit operator bool(ItemStack itemStack)
        {
            return itemStack.Item != null;
        }
    }
}
