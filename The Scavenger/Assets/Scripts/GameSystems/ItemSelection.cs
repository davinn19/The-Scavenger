using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(ItemBuffer))]
    public class ItemSelection : MonoBehaviour
    {
        /// <remarks>DO NOT EDIT DIRECTLY. Use other methods to change the held item.</remarks>
        [field: SerializeField] public ItemBuffer HeldItemBuffer { get; private set; }

        public ItemStack GetHeldItem()
        {
            return HeldItemBuffer.GetItemInSlot(0);
        }

        public bool IsEmpty()
        {
            ItemStack heldItem = GetHeldItem();
            return !heldItem || heldItem.IsEmpty();
        }

        /// <summary>
        /// Extracts from the held item buffer.
        /// </summary>
        /// <param name="amount">Amount of items to use.</param>
        /// <returns>Amount of items actually used.</returns>
        public int Use(int amount = 1)
        {
            return HeldItemBuffer.Extract(0, amount);
        }

        /// <summary>
        /// Moves items from held item buffer to another buffer.
        /// </summary>
        /// <param name="otherBuffer">Buffer to insert items into.</param>
        /// <param name="slot">Slot to insert into.</param>
        /// <returns>Amount of items moved.</returns>
        public int MoveItemsTo(ItemBuffer otherBuffer, int slot)
        {
            return ItemBuffer.MoveItems(HeldItemBuffer, 0, otherBuffer, slot);
        }

        /// <summary>
        /// Moves items from another buffer to held item buffer.
        /// </summary>
        /// <param name="otherBuffer">Buffer to extract items from.</param>
        /// <param name="slot">Slot to extract from.</param>
        /// <returns>Amount of items moved.</returns>
        public int TakeItemsFrom(ItemBuffer otherBuffer, int slot)
        {
            return ItemBuffer.MoveItems(otherBuffer, slot, HeldItemBuffer, 0);
        }

        /// <summary>
        /// Swaps itemStacks between held item buffer and a slot in another buffer.
        /// </summary>
        /// <param name="otherBuffer">Buffer to swap with.</param>
        /// <param name="slot">Slot to swap with.</param>
        public void Swap(ItemBuffer otherBuffer, int slot)
        {
            ItemBuffer.Swap(HeldItemBuffer, 0, otherBuffer, slot);
        }
    }
}
