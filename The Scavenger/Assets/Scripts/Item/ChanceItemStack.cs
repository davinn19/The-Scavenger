using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class ChanceItemStack : ItemStack, ChanceRecipeComponent<ItemStack>
    {
        private float chance;
        public float GetChance() => chance;

        public ChanceItemStack(Item item, float chance, int amount = 1) : base(item, amount)
        {
            this.chance = chance;
        }

        public ChanceItemStack(string itemID, float chance, int amount = 1) : base(itemID, amount)
        {
            this.chance = chance;
        }
    }
}
