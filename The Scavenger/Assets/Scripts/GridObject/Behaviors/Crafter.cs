using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs, make more efficient
    public class Crafter : GridObjectBehavior
    {
        [field: SerializeField] public bool MakeshiftOnly { get; private set; }

        // TODO add docs, make more efficient
        public int Craft(CraftingRecipe recipe, int requestedYield, PlayerInventory inventory)
        {
            if (recipe == null)
            {
                return 0;
            }

            requestedYield = Mathf.Min(requestedYield, GetRecipeMaxYield(recipe, inventory));

            List<ItemStack> inventorySlots = inventory.GetInventory();
            int actualYield = 0;

            // Inserts as much result as possible without overfilling
            while (actualYield < requestedYield)
            {
                ItemStack resultProduced = recipe.Output.Clone();
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
            foreach (RecipeComponent<ItemStack> ingredient in recipe.Inputs)
            {
                ItemTransfer.ExtractFromBuffer(inventorySlots, ingredient.CanSubstituteWith, actualYield * ingredient.Amount, false);
            }

            return actualYield;
        }


        // TODO implement, add docs
        public List<CraftingRecipe> GetRecipes(string searchInput, FilterMode filterMode, PlayerInventory inventory)
        {
            List<CraftingRecipe> results = new();

            foreach (CraftingRecipe recipe in GetRecipes())
            {
                // Check if recipe result name matches search
                string resultName = recipe.Output.Item.DisplayName;
                if (!resultName.Contains(searchInput, System.StringComparison.OrdinalIgnoreCase))
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
                        if (InventoryContainsIngredient(recipe, inventory))
                        {
                            results.Add(recipe);
                        }
                        break;
                }
            }

            return results;
        }

        // TODO add docs
        private List<CraftingRecipe> GetRecipes()
        {
            if (MakeshiftOnly)
            {
                return CraftingRecipes.Instance.GetMakeshiftRecipes();
            }
            else
            {
                return CraftingRecipes.Instance.GetAllRecipes();
            }
        }

        /// <summary>
        /// Gets the most times a recipe can be used with the available items in the inventory.
        /// </summary>
        /// <param name="recipe">The recipe to calculate yield for.</param>
        /// <param name="inventory">The player's inventory.</param>
        /// <returns>The recipe's maximum yield.</returns>
        public int GetRecipeMaxYield(CraftingRecipe recipe, PlayerInventory inventory)
        {
            int smallestMaxYield = int.MaxValue;
            foreach (RecipeComponent<ItemStack> ingredient in recipe.Inputs)
            {
                int requiredAmount = ingredient.Amount;
                int availableAmount = ItemTransfer.ExtractFromBuffer(inventory.GetInventory(), ingredient.CanSubstituteWith, int.MaxValue, true);

                int yield = availableAmount / requiredAmount;

                if (yield == 0)
                {
                    return 0;
                }

                smallestMaxYield = Mathf.Min(smallestMaxYield, yield);
            }

            return smallestMaxYield;
        }

        // TODO add docs
        public bool InventoryContainsIngredient(CraftingRecipe recipe, PlayerInventory inventory)
        {
            foreach (ItemStack inventorySlot in inventory.GetInventory())
            {
                foreach (RecipeComponent<ItemStack> ingredient in recipe.Inputs)
                {
                    if (ingredient.CanSubstituteWith(inventorySlot))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public enum FilterMode
        {
            All,
            HaveIngredient,
            Craftable
        }
    }
}
    