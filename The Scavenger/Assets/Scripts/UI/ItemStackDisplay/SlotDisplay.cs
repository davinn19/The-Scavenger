using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scavenger.GridObjectBehaviors;

namespace Scavenger.UI
{
    /// <summary>
    /// Displays the itemStack in one slot of an item buffer.
    /// </summary>
    [RequireComponent(typeof(ItemStackDisplay))]
    public class SlotDisplay : MonoBehaviour
    {
        public ItemBuffer Buffer { get; private set; }
        public ItemStack ManagedItemStack { get; private set; }

        public bool Insertable = true;// TODO factor these in
        public bool Extractable = true;
        public bool Swappable => Insertable && Extractable;

        public ItemStackDisplay ItemStackDisplay { get; private set; }
        private SlotDisplayHandler handler;

        private void Awake()
        {
            ItemStackDisplay = GetComponent<ItemStackDisplay>();
            handler = GetComponentInParent<SlotDisplayHandler>();
        }

        /// <summary>
        /// Changes which item buffer and slot to display.
        /// </summary>
        /// <param name="buffer">The item buffer to display.</param>
        /// <param name="itemStack">The slot to display.</param>
        public void SetWatchedBuffer(ItemBuffer buffer, ItemStack itemStack)
        {
            Debug.Assert(buffer.ItemStackIsInBuffer(itemStack));
            Buffer = buffer;
            ManagedItemStack = itemStack;

            ItemStackDisplay.ItemStack = ManagedItemStack;
        }

        // TODO add docs
        public void OnPointerClick()
        {
            handler.OnSlotDisplayPressed(this);
        }
    }
}
