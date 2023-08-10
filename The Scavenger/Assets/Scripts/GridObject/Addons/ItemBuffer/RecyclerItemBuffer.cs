using Leguar.TotalJSON;
using Scavenger.GridObjectBehaviors;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs, implement
    public class RecyclerItemBuffer : ItemBuffer
    {
        private const int inputSlots = 1;
        private const int outputSlots = 15;

        private ItemStack[] items = new ItemStack[inputSlots + outputSlots];

        public override ItemStack[] GetItems() => items;
        public override void SetItems(ItemStack[] items) => this.items = items;
        public override void Clear() => items = new ItemStack[inputSlots + outputSlots];

        public override bool CanExtract(int slot) => !CanInsert(slot);
        public override bool CanInsert(int slot) => slot < inputSlots;

        public override bool AcceptsItemStack(int slot, ItemStack itemStack)
        {
            if (CanInsert(slot))
            {
                return GetComponent<Recycler>().RecipeList.IsRecyclable(itemStack.Item);
            }
            return false;
        }

        public ItemStack GetInput()
        {
            return items[0];
        }

        /// <summary>
        /// Checks if the recycler's output slots are all full.
        /// </summary>
        /// <returns></returns>
        public bool IsOutputFull()
        {
            for (int slot = 1; slot < GetNumSlots(); slot++)
            {
                if (!IsSlotFull(slot))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
