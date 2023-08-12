using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    /// <summary>
    /// Displays the itemStack in one slot of an item buffer.
    /// </summary>
    [RequireComponent(typeof(ItemStackDisplay))]
    public class SlotDisplay : MonoBehaviour
    {
        public ItemBuffer Buffer { get; private set; }

        public bool Insertable = true;// TODO factor these in
        public bool Extractable = true;

        private ItemStackDisplay itemStackDisplay;
        private SlotDisplayHandler handler;

        private void Awake()
        {
            itemStackDisplay = GetComponent<ItemStackDisplay>();
            handler = GetComponentInParent<SlotDisplayHandler>();
        }

        /// <summary>
        /// Changes which item buffer and slot to display.
        /// </summary>
        /// <param name="buffer">The item buffer to display.</param>
        /// <param name="slot">The slot to display.</param>
        public void SetWatchedBuffer(ItemBuffer buffer, int slot) => SetWatchedBuffer(buffer, buffer.GetItemInSlot(slot));

        public void SetWatchedBuffer(ItemBuffer buffer, ItemStack itemStack)
        {
            Debug.Assert(buffer.ItemStackIsInBuffer(itemStack));
            Buffer = buffer;
            itemStackDisplay.ItemStack = itemStack;
        }

        // TODO add docs
        public void OnPointerClick()
        {
            handler.OnSlotDisplayPressed(this);
        }

        /// <summary>
        /// Gets the itemStack in the displayed item buffer and slot.
        /// </summary>
        /// <returns></returns>
        public ItemStack GetDisplayedItemStack()
        {
            return itemStackDisplay.ItemStack;
        }

    }
}
