using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Recipes
{
    // TODO implement, add docs
    public class AssemblerRecipes : RecipeList<AssemblerRecipe>
    {
        private static AssemblerRecipes instance = new();
        public static AssemblerRecipes Instance => instance;

        private List<AssemblerRecipe> recipes = new();

        public override List<Recipe> GetGenericRecipes() => recipes.ConvertAll<Recipe>((recipe) => recipe);

        public override List<AssemblerRecipe> GetRecipes() => recipes;

        public AssemblerRecipe GetRecipeWithInput(ItemStack baseItem, ItemStack[] addons)
        {
            // Ignore empty addon slots
            addons = System.Array.FindAll(addons, (addon) => addon);

            foreach (AssemblerRecipe recipe in recipes)
            {
                // Base input must match recipe
                if (!recipe.baseItem.CanSubstituteWith(baseItem))
                {
                    continue;
                }

                // Number of addons must be equal to the amount of specified addons in the recipe
                if (recipe.addons.Length != addons.Length)
                {
                    continue;
                }

                // Keeps track of different combinations of addons to satisfy each recipe component
                Queue<List<int>> addonUsages = new();
                addonUsages.Enqueue(new());

                // Check for matches in the other recipe components, not matching with already matched addons
                for (int recipeAddonIndex = 0; recipeAddonIndex < recipe.addons.Length; recipeAddonIndex++)
                {
                    // End early if no combinations are available
                    if (addonUsages.Count == 0)
                    {
                        break;
                    }

                    RecipeComponent<ItemStack> recipeAddon = recipe.addons[recipeAddonIndex];

                    // Looping through all remaining usage combinations
                    int numAddonUsages = addonUsages.Count;
                    for (int addonUsageIndex = 0; addonUsageIndex < numAddonUsages; addonUsageIndex++)
                    {
                        List<int> addonUsage = addonUsages.Dequeue();

                        // Checks if any addon not already in the combination can substitute the current recipe addon
                        for (int addonIndex = 0; addonIndex < addons.Length; addonIndex++)
                        {
                            // Skips if addon is already used in the current combination
                            if (addonUsage.Contains(addonIndex))
                            {
                                continue;
                            }

                            // Skips if addon cannot satisfy the recipe component
                            if (!recipeAddon.CanSubstituteWith(addons[addonIndex]))
                            {
                                continue;
                            }

                            // If adding to the combinations satisfies the full recipe, just return instead.
                            if (recipeAddonIndex == recipe.addons.Length - 1)
                            {
                                return recipe;
                            }
                            else    // Otherwise, add a new combination with the addon at the end
                            {
                                addonUsages.Enqueue(new(addonUsage) { addonIndex });
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void AddRecipe(RecipeComponent<ItemStack> baseItem, RecipeComponent<ItemStack>[] addons, ItemStack output)
        {
            // Checks if all components have an amount of 1
            Debug.Assert(baseItem.Amount == 1);
            Debug.Assert(output.Amount == 1);
            Debug.Assert(addons.Length <= 4);

            foreach (RecipeComponent<ItemStack> addon in addons)
            {
                Debug.Assert(addon.Amount == 1);
            }

            // Checks for any recipe conflicts
            Debug.Assert(GetRecipeWithInput(baseItem.GetRecipeComponent(), System.Array.ConvertAll(addons, (component) => component.GetRecipeComponent())) == null);

            recipes.Add(new AssemblerRecipe(baseItem, addons, output));
        }

        public override void LoadRecipes()
        {
            // TODO add recipes
            AddRecipe(
                new ItemStack("AluminumFrame"),
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("AluminumPlate"),
                    new ItemStack("AluminumPlate"),
                    new ItemStack("AluminumPlate"),
                    new ItemStack("AluminumPlate"),
                },
                new ItemStack("BasicSilo")
                );

            AddRecipe(
                new ItemStack("AluminumFrame"),
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("RechargableBattery"),
                    new ItemStack("RechargableBattery"),
                    new ItemStack("RechargableBattery"),
                    new ItemStack("CopperScrap"),
                },
                new ItemStack("BasicEnergyCell")
                );

            AddRecipe(
                new ItemStack("AluminumFrame"),
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("AluminumPlate"),
                    new ItemStack("PolySiliconWafer"),
                    new ItemStack("PolySiliconWafer"),
                    new ItemStack("PolySiliconWafer"),
                },
                new ItemStack("BasicSolarPanel")
                );
        }

    }
}
