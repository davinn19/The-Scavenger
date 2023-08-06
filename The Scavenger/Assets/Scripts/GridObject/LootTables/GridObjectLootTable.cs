using Leguar.TotalJSON;
using Scavenger.GridObjectBehaviors;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [RequireComponent(typeof(GridObjectBehavior))]
    public class GridObjectLootTable : LootTable
    {
        [SerializeField] Item droppedItem;
        private GridObjectBehavior behavior;

        private void Awake()
        {
            behavior = GetComponent<GridObjectBehavior>();
        }

        public override List<ItemStack> GetDrops()
        {
            JSON persistentData = behavior.WritePersistentData();
            ItemStack droppedItemStack = new ItemStack(droppedItem, 1, persistentData);
            return new List<ItemStack>() { droppedItemStack };
        }

    }
}
