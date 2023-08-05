using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Populates an item buffer.
    /// </summary>
    [System.Serializable]
    public struct ItemStack
    {
        [field: SerializeField] public Item Item { get; private set; }

        /// <remarks>DO NOT edit the amount directly unless you are in ItemBuffer. Use Extract/Insert functions in ItemBuffer instead.</remarks>
        [Min(0)] public int Amount;
        public JSON PersistentData;

        // TODO add docs
        public ItemStack(Item item, int amount, JSON persistentData = null)
        {
            Item = item;
            Amount = amount;
            PersistentData = persistentData;
        }

        /// <summary>
        /// Creates a copy of another itemStack with a different amount.
        /// </summary>
        /// <param name="otherStack">The itemStack to copy.</param>
        /// <param name="newAmount">The amount the new itemStack should have.</param>
        public ItemStack(ItemStack otherStack, int newAmount)
        {
            Item = otherStack.Item;
            Amount = newAmount;

            if (otherStack.PersistentData != null)
            {
                PersistentData = new JSON(otherStack.PersistentData.AsDictionary());
            }
            else
            {
                PersistentData = null;
            }
        }

        /// <summary>
        /// Creates a copy of another itemStack.
        /// </summary>
        /// <param name="otherStack">The itemStack to copy.</param>
        public ItemStack(ItemStack otherStack) : this(otherStack.Item, otherStack.Amount) { }

        public static readonly ItemStack Empty = new(null, 0);

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
        /// Chekcs if the itemStack contains persistent data.
        /// </summary>
        /// <returns>True if the itemStack contains persistent data.</returns>
        public bool HasPersistentData()
        {
            return PersistentData != null && PersistentData.Count > 0;
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
