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
        [field: SerializeField] public ItemBuffer Buffer { get; private set; }
        [field: SerializeField] public int Slot { get; private set; }

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
        public void SetWatchedBuffer(ItemBuffer buffer, int slot)
        {
            if (Buffer)
            {
                buffer.SlotChanged -= OnSlotChanged;
            }

            Buffer = buffer;
            Slot = slot;

            if (Buffer)
            {
                buffer.SlotChanged -= OnSlotChanged;
            }
        }

        // TODO add docs
        private void OnSlotChanged(int slotChanged)
        {
            if (slotChanged != Slot)
            {
                return;
            }

            itemStackDisplay.ItemStack = GetItemInSlot();
        }

        // TODO add docs
        public void OnPointerClick()
        {
            handler.OnSlotDisplayPressed(this);
        }

        /// <summary>
        /// Constantly update which itemStack the display should be watching
        /// </summary>
        private void Update()
        {
            if (Buffer == null)
            {
                itemStackDisplay.ItemStack = ItemStack.Empty;
                return;
            }

            itemStackDisplay.ItemStack = Buffer.GetItemInSlot(Slot);
        }

        /// <summary>
        /// Gets the itemStack in the displayed item buffer and slot.
        /// </summary>
        /// <returns></returns>
        public ItemStack GetItemInSlot()
        {
            return Buffer.GetItemInSlot(Slot);
        }

    }
}
