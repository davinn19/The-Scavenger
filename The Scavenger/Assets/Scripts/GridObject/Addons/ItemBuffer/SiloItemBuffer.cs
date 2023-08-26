using Leguar.TotalJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs
    public class SiloItemBuffer : ItemBuffer
    {
        [SerializeField] private int MaxStackAmount = 100;

        private ItemStack storedItem = new ItemStack();
        public ItemStack StoredItem => storedItem;

        // TODO override maxstacksize
        public ItemStack lockFilter = new ItemStack();
        public override int NumSlots => 1;

        protected override void Init()
        {
            base.Init();
            storedItem.SetMaxAmount(MaxStackAmount);
        }


        public override ItemStack GetItemInSlot(int slot)
        {
            if (slot != 0)
            {
                throw new ArgumentOutOfRangeException("A silo only has one slot, so the only valid slot is 0.");
            }
            return storedItem;
        }
        // Always returns stored item because conduit interaction is unrestricted.
        public override List<ItemStack> GetAllSlots() => new List<ItemStack>() { storedItem };
        public override List<ItemStack> GetInputSlots() => GetAllSlots();
        public override List<ItemStack> GetOutputSlots() => GetAllSlots();

        /// <summary>
        /// Checks if the inserting itemStack matches the filter.
        /// </summary>
        /// <param name="itemStack">The itemStack to be inserted.</param>
        /// <returns>True if the itemStack matches the silo's filter.</returns>
        public override bool AcceptsItemStack(ItemStack _, ItemStack itemStack)
        {
            return lockFilter.IsStackable(itemStack);
        }

        // TODO add docs
        public bool IsLocked() => lockFilter;

        /// <summary>
        /// Switches the silo's locked status.
        /// </summary>=
        public void ToggleLocked()
        {
            if (lockFilter)
            {
                lockFilter.Clear();
            }
            else
            {
                lockFilter.Copy(storedItem, false);
                lockFilter.SetAmount(1);
            }
        }

        /// <summary>
        /// Also checks if the silo should be locked.
        /// </summary>
        /// <param name="data">The buffer's persistent data.</param>
        public override void ReadPersistentData(JSON data)
        {
            if (data.ContainsKey("LockFilter"))
            {
                lockFilter = data.GetJSON("LockFilter").Deserialize<ItemStack>();
            }
            if (data.ContainsKey("Item"))
            {
                storedItem = data.GetJSON("Item").Deserialize<ItemStack>();
            }
        }

        /// <summary>
        /// Also saves when the silo is locked.
        /// </summary>
        /// <returns>The buffer's persistent data.</returns>
        public override JSON WritePersistentData()
        {
            JSON data = new JSON();
            if (lockFilter)
            {
                data.Add("LockFilter", lockFilter);
            }
            if (storedItem)
            {
                data.Add("Item", storedItem);
            }

            return data;
        }
    }
}
