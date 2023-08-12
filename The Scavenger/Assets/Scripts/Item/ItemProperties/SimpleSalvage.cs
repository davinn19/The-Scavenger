using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Allows an item to be salvaged from the inventory for few materials.
    /// </summary>
    public class SimpleSalvage : Interactibe    // TODO use recycle recipes
    {
        public override string Name => "Simple Salvage";

        [SerializeField] private Item[] salvageDrops;
        [SerializeField, Range(0, 1)] private float dropChance;

        /// <summary>
        /// Salvages one of the items in the stack.
        /// </summary>
        /// <param name="gameManager">The current game manager.</param>
        /// <param name="pressedPos">The grid position pressed.</param>
        /// 
        public override void Interact(GameManager gameManager, Vector2Int pressedPos)
        {
            GridMap map = gameManager.Map;

            if (map.IsSupported(pressedPos) && !map.GetObjectAtPos(pressedPos)) // Needs to be pressed on an EMPTY spot on the asteroid
            {
                foreach (ItemStack drop in GenerateDrops())
                {
                    Vector2 worldPos = gameManager.GetComponent<InputHandler>().HoveredWorldPos;
                    gameManager.ItemDropper.CreateFloatingItem(drop, worldPos);
                }

                gameManager.Inventory.UseHeldItem();
            }
        }

        /// <summary>
        /// Gets the drops for an interaction.
        /// </summary>
        /// <returns>List of item drops.</returns>
        private List<ItemStack> GenerateDrops()
        {
            List<ItemStack> drops = new();

            foreach (Item drop in salvageDrops)
            {
                if (Random.value < dropChance)
                {
                    drops.Add(new ItemStack(drop, 1, new JSON()));
                }
            }

            return drops;
            
        }
    }
}
