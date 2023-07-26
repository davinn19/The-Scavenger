using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class SimpleSalvage : Interactibe
    {
        public override string Name => "Simple Salvage";

        [SerializeField] private Item[] salvageDrops;

        public override void Interact(GameManager gameManager, ItemSelection itemSelection, Vector2Int pressedPos)
        {
            GridMap map = gameManager.Map;

            if (map.IsSupported(pressedPos) && !map.GetObjectAtPos(pressedPos)) // Needs to be pressed on an EMPTY spot on the asteroid
            {
                foreach (ItemStack drop in GenerateDrops())
                {
                    Vector2 worldPos = gameManager.GetComponent<GridHover>().WorldPos;
                    gameManager.ItemDropper.CreateFloatingItem(drop, worldPos);
                }

                itemSelection.Use();
            }
        }

        private List<ItemStack> GenerateDrops()
        {
            List<ItemStack> drops = new();

            foreach (Item drop in salvageDrops)
            {
                if (Random.value < 0.25f)
                {
                    drops.Add(new ItemStack(drop, 1));
                }
            }

            return drops;
            
        }
    }
}
