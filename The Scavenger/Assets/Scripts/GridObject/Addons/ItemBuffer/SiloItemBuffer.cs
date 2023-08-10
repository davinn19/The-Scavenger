using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class SiloItemBuffer : ItemBuffer
    {
        private ItemStack[] item = new ItemStack[1];
        public override int MaxStackSize => base.MaxStackSize;
        [field: SerializeField] public bool Locked { get; private set; }

        public override ItemStack[] GetItems() => item;
        public override ItemStack[] GetItemsPartial(SlotType slotType)
        {

        }

        public override void SetItems(ItemStack[] items) => item = items;
        public override void Clear() => item = new ItemStack[1];
        public override bool CanExtract(int slot) => true;
        public override bool CanInsert(int slot) => true;
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
            ItemStack extractedStack = GetItemInSlot(slot);

            // Ignore if extracted stack is empty
            if (!extractedStack)
            {
                return 0;
            }

            int amountExtracted = Mathf.Min(extractedStack.Amount, amount);

            extractedStack.Amount -= amountExtracted;
            if (extractedStack.IsEmpty() || !Locked)
            {
                extractedStack = default;
            }

            SetItemInSlot(slot, extractedStack);
            InvokeSlotChanged(slot);

            return amountExtracted;
        }

        /// <summary>
        /// Also checks if the silo should be locked.
        /// </summary>
        /// <param name="data">The buffer's persistent data.</param>
        public override void ReadPersistentData(JSON data)
        {
            base.ReadPersistentData(data);
            if (data.ContainsKey("Locked"))
            {
                Locked = true;
            }
        }

        /// <summary>
        /// Also saves when the silo is locked.
        /// </summary>
        /// <returns>The buffer's persistent data.</returns>
        public override JSON WritePersistentData()
        {
            JSON data = base.WritePersistentData();
            if (Locked)
            {
                data.Add("Locked", true);
            }

            return data;
        }
    }
}
