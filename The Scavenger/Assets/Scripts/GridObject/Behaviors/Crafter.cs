using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs
    public class Crafter : GridObjectBehavior
    {
        [field: SerializeField] public CraftingRecipes RecipeList { get; private set; }

        // TODO implement, add docs
        public int Craft(CraftingRecipe recipe, int requestedYield, ItemBuffer inventory)
        {
            if (!recipe)
            {
                return 0;
            }
            // TODO fix
            //requestedYield = Mathf.Min(requestedYield, GetRecipeMaxYield(recipe, inventory));

            //ItemStack resultProduced = recipe.Result.Clone();
            //int actualYield = 0;

            //// Inserts as much result as possible without overfilling
            //while (actualYield < requestedYield)
            //{
            //    int uninsertedResult = inventory.Insert(resultProduced, simulate: true);

            //    if (uninsertedResult > 0)
            //    {
            //        break;
            //    }

            //    inventory.Insert(resultProduced);
            //    actualYield++;
            //}

            //// Extract actual amount used
            //foreach (ItemStack ingredient in recipe.Ingredients)
            //{
            //    ItemTransfer.ExtractItem(ingredient.Item)
            //    inventory.Extract(ingredient.Item, actualYield * ingredient.Amount);
            //}

            //return actualYield;
            return 0;
        }


        // TODO implement, add docs
        public List<CraftingRecipe> GetRecipes(string searchInput, FilterMode filterMode, ItemBuffer inventory)
        {
            List<CraftingRecipe> results = new();

            foreach (CraftingRecipe recipe in RecipeList.GetRecipes())
            {
                // Check if recipe result name matches search
                string resultName = recipe.Result.Item.DisplayName.ToLower();
                if (!resultName.Contains(searchInput.ToLower()))
                {
                    continue;
                }

                // Run through filter
                switch (filterMode)
                {
                    case FilterMode.All:
                        results.Add(recipe);
                        break;
                    case FilterMode.Craftable:
                        if (GetRecipeMaxYield(recipe, inventory) > 0)
                        {
                            results.Add(recipe);
                        }
                        break;
                    case FilterMode.HaveIngredient:
                        if (InventoryHasIngredient(recipe, inventory))
                        {
                            results.Add(recipe);
                        }
                        break;
                }
            }

            return results;
        }

        /// <summary>
        /// Checks if the player's inventory contains at least 1 ingredient in the recipe.
        /// </summary>
        /// <param name="recipe">The recipe to check.</param>
        /// <param name="inventory">The player's inventory.</param>
        /// <returns>True if the inventory contains at least 1 ingredient.</returns>
        private bool InventoryHasIngredient(CraftingRecipe recipe, ItemBuffer inventory)
        {
            foreach (ItemStack ingredient in recipe.Ingredients)
            {
                int availableAmount = inventory.Extract(ingredient.Item, simulate: true);
                if (availableAmount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the most times a recipe can be used with the available items in the inventory.
        /// </summary>
        /// <param name="recipe">The recipe to calculate yield for.</param>
        /// <param name="inventory">The player's inventory.</param>
        /// <returns>The recipe's maximum yield.</returns>
        public int GetRecipeMaxYield(CraftingRecipe recipe, ItemBuffer inventory)
        {
            int smallestMaxYield = int.MaxValue;
            foreach (ItemStack ingredient in recipe.Ingredients)
            {
                int requiredAmount = ingredient.Amount;
                int availableAmount = ItemTransfer.ExtractFromBuffer(inventory.GetItems(), ingredient.SharesItem, int.MaxValue, true);

                int yield = availableAmount / requiredAmount;

                if (yield == 0)
                {
                    return 0;
                }

                smallestMaxYield = Mathf.Min(smallestMaxYield, yield);
            }

            return smallestMaxYield;
        }


        public enum FilterMode
        {
            All,
            HaveIngredient,
            Craftable
        }
    }
}
    