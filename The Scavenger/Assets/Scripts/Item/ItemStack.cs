using Leguar.TotalJSON;
using System;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Populates an item buffer.
    /// </summary>
    [System.Serializable]
    public class ItemStack : ResourceStack, RecipeComponent<ItemStack>
    {
        // Item things
        public virtual Item Item => ItemDatabase.GetItem(ID);
        public override int DefaultMaxAmount => 100;
        

        [SerializeField] private JSON persistentData = new JSON();
        public JSON PersistentData => persistentData;

        public void SetPersistentData(JSON data)
        {
            persistentData = JSONHelper.Copy(data);
            persistentData.SetProtected();
            OnChange();
        }

        public ItemStack()
        {
            ID = "";
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
            ID = item.name;
        }

        public ItemStack(Item item, int amount) : this(item, amount, new JSON()) { }


        public ItemStack(string itemID, int amount = 1) : this(ItemDatabase.GetItem(itemID), amount) { }


        /// <summary>
        /// Copies the information of another itemStack. Will not copy max amount.
        /// </summary>
        /// <param name="copyAmount">If true, copy the itemStack's amount.</param>
        /// <param name="copyPersistentData">If true, copy the persistent data.</param>
        public void Copy(ItemStack itemStackToCopy, bool copyAmount = true, bool copyPersistentData = true)
        {
            ID = itemStackToCopy.ID;
            if (copyAmount)
            {
                amount = Mathf.Clamp(itemStackToCopy.amount, 0, MaxAmount);
            }
            if (copyPersistentData)
            {
                persistentData = JSONHelper.Copy(itemStackToCopy.persistentData);
                persistentData.SetProtected();
            }
            OnChange();
        }

        /// <summary>
        /// Creates a copy of another itemStack. Max amount is NOT copied.
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
            return copy;
        }

        // TODO add docs
        public override void Clear()
        {
            persistentData = new JSON();
            persistentData.SetProtected();
            base.Clear();
            ID = "";
            amount = 0;
            
            OnChange();
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
            if (other.Item != Item)
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
        [Obsolete]
        public bool SharesItem(ItemStack other)
        {
            return other.Item == Item;
        }

        /// <summary>
        /// Checks if another itemStack can substitute this in a recipe.
        /// </summary>
        /// <param name="other">ItemStack to substitute with.</param>
        /// <returns>True if the itemStack can be substituted.</returns>
        public override bool CanSubstituteWith(RecipeComponent other)
        {
            if (other is not ItemStack)
            {
                return false;
            }

            return base.CanSubstituteWith(other);
        }

        // TODO add docs
        public virtual ItemStack GetRecipeComponent()
        {
            return Clone();
        }
    }
}
