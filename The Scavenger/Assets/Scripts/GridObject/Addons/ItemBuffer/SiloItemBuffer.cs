using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Scavenger.ItemTransfer;

namespace Scavenger
{
    // TODO add docs
    public class SiloItemBuffer : ItemBuffer
    {
        private ItemStack storedItem = new ItemStack();
        // TODO override maxstacksize
        [field: SerializeField] public bool Locked { get; private set; }

        public override List<int> GetInteractibleSlots(ItemInteractionType interactionType) => new List<int> { 1 };

        public override List<ItemStack> GetItems(ItemInteractionType interactionType = ItemInteractionType.All) => new List<ItemStack>() { storedItem };

        public override bool AcceptsItemStack(int slot, ItemStack itemStack) => true;

        /// <summary>
        /// Switches a slot's locked status.
        /// </summary>
        /// <param name="slot">The slot to toggle.</param>
        public void ToggleLocked()
        {
            Locked = !Locked;
        }

        /// <summary>
        /// Similar to base extract, but accounts for locked status before clearing an empty itemStack.
        /// </summary>
        /// <param name="slot">Slot to extract items from.</param>
        /// <param name="amount">Amount of items to extract.</param>
        /// <returns>Actual amount of items extracted.</returns>
        public override int ExtractSlot(int slot, int amount)
        {
            ItemStack emptyReserve = GetItemInSlot(slot).Clone(amount: 0);
            int amountExtracted = base.ExtractSlot(slot, amount);

            if (!GetItemInSlot(slot))
            {
                SetItemInSlot(slot, emptyReserve);
            }

            return amountExtracted;
        }

        /// <summary>
        /// Also checks if the silo should be locked.
        /// </summary>
        /// <param name="data">The buffer's persistent data.</param>
        public override void ReadPersistentData(JSON data)
        {
            if (data.ContainsKey("Locked"))
            {
                Locked = true;
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
            if (Locked)
            {
                data.Add("Locked", true);
            }
            if (storedItem)
            {
                data.Add("Item", storedItem);
            }

            return data;
        }
    }
}
