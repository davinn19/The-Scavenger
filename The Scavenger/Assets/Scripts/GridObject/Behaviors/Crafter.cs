using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs, make more efficient
    public class Crafter : GridObjectBehavior
    {
        [field: SerializeField] public CraftingRecipes RecipeList { get; private set; }

        // TODO add docs, make more efficient
        public int Craft(CraftingRecipe recipe, int requestedYield, PlayerInventory inventory)
        {
            if (!recipe)
            {
                return 0;
            }

            requestedYield = Mathf.Min(requestedYield, GetRecipeMaxYield(recipe, inventory));

            List<ItemStack> inventorySlots = inventory.GetInventory();
            int actualYield = 0;

            // Inserts as much result as possible without overfilling
            while (actualYield < requestedYield)
            {
                ItemStack resultProduced = recipe.Result.Clone();
                int insertedResult = ItemTransfer.MoveStackToBuffer(resultProduced, inventorySlots, resultProduced.Amount, inventory.AcceptsItemStack, true);

                // Has to be able to insert everything to craft, TODO change somehow??? dropping excess maybe
                if (insertedResult != resultProduced.Amount)
                {
                    break;
                }

                ItemTransfer.MoveStackToBuffer(resultProduced, inventory.GetInventory(), resultProduced.Amount, inventory.AcceptsItemStack, false);
                actualYield++;
            }

            // Extract actual amount used
            foreach (ItemStack ingredient in recipe.Ingredients)
            {
                ItemTransfer.ExtractFromBuffer(inventorySlots, (itemStack) => itemStack.SharesItem(ingredient), actualYield * ingredient.Amount, false);
            }

            return actualYield;
        }


        // TODO implement, add docs
        public List<CraftingRecipe> GetRecipes(string searchInput, FilterMode filterMode, PlayerInventory inventory)
        {
            List<CraftingRecipe> results = new();
            List<ItemStack> inventorySlots = inventory.GetInventory();


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
                        if (ItemTransfer.BufferContainsAnyItem(inventorySlots, recipe.Ingredients))
                        {
                            results.Add(recipe);
                        }
                        break;
                }
            }

            return results;
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
                int availableAmount = ItemTransfer.ExtractFromBuffer(inventory.GetAllSlots(), ingredient.SharesItem, int.MaxValue, true);

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
    