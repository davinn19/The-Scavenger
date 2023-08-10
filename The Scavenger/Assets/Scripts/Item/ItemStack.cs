using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Populates an item buffer.
    /// </summary>
    [System.Serializable]
    public class ItemStack
    {
        [SerializeField] private string itemID;

        public Item Item => Database.Instance.Item.Get(itemID);

        /// <remarks>DO NOT edit the amount directly unless you are in ItemBuffer. Use Extract/Insert functions in ItemBuffer instead.</remarks>
        [Min(0)] public int Amount = 0;

        public JSON PersistentData = new JSON();

        public ItemStack()
        {
            itemID = "";
            Amount = 0;
            PersistentData = new JSON();
        }

        // TODO add docs
        public ItemStack(Item item, int amount, JSON persistentData)
        {
            Amount = amount;
            PersistentData = JSONHelper.GetJSONOrEmpty(persistentData); // TODO convert to normal???
            itemID = item.name;
        }

        /// <summary>
        /// Copies the information of another itemStack.
        /// </summary>
        /// <param name="copyAmount">If true, copy the itemStack's amount.</param>
        /// <param name="copyPersistentData">If true, copy the persistent data.</param>
        public void Copy(ItemStack itemStackToCopy, bool copyAmount = true, bool copyPersistentData = true)
        {
            itemID = itemStackToCopy.itemID;
            if (copyAmount)
            {
                Amount = itemStackToCopy.Amount;
            }
            if (copyPersistentData)
            {
                PersistentData = JSONHelper.Copy(itemStackToCopy.PersistentData);
            }
        }

        /// <summary>
        /// Creates a copy of another itemStack.
        /// </summary>
        /// <param name="amount">Set the copy's amount to something different.</param>
        /// <param name="keepPersistentData">If true, copy the persistent data.</param>
        /// <returns>The itemStack copy.</returns>
        public ItemStack Clone(int amount = -1, bool keepPersistentData = true)
        {
            if (amount < 0)
            {
                amount = Amount;
            }

            JSON persistentData = new JSON();
            if (keepPersistentData)
            {
                JSONHelper.Copy(JSONHelper.GetJSONOrEmpty(PersistentData));
            }

            ItemStack copy = new ItemStack(Item, amount, persistentData);
            return copy;
        }

        // TODO add docs
        public void Clear()
        {
            itemID = "";
            Amount = 0;
            PersistentData = new JSON();
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
            if (Item != other.Item)
            {
                return false;
            }

            // Stacks need to both have/not have persistent data
            if (HasPersistentData() != other.HasPersistentData())
            {
                return false;
            }

            // If the stacks' data are different, it is unstackable
            if (HasPersistentData() && !PersistentData.Equals(other.PersistentData))
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
            return PersistentData != null && PersistentData.Count > 0;
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
            return Amount <= 0;
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
