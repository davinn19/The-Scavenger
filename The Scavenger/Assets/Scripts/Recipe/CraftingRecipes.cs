using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class CraftingRecipes : RecipeList<CraftingRecipe>
    {
        private static CraftingRecipes instance = new CraftingRecipes();
        public static CraftingRecipes Instance => instance;

        private readonly List<CraftingRecipe> allRecipes = new();
        private readonly List<CraftingRecipe> makeshiftRecipes = new();

        public List<CraftingRecipe> GetRecipesWithInput(RecipeComponent input)
        {
            List<CraftingRecipe> recipesWithInput = new();
            foreach (CraftingRecipe recipe in allRecipes)
            {
                if (recipe.IsInput(input))
                {
                    recipesWithInput.Add(recipe);
                }
            }
            return recipesWithInput;
        }

        public List<CraftingRecipe> GetRecipesWithOutput(RecipeComponent output)
        {
            List<CraftingRecipe> recipesWithOutput = new();
            foreach (CraftingRecipe recipe in allRecipes)
            {
                if (recipe.IsOutput(output))
                {
                    recipesWithOutput.Add(recipe);
                }
            }
            return recipesWithOutput;
        }

        public List<Recipe> GetRecipes()
        {
            return allRecipes.ConvertAll<Recipe>((craftingRecipe) => craftingRecipe);
        }

        public List<CraftingRecipe> GetMakeshiftRecipes()
        {
            return makeshiftRecipes;
        }

        public List<CraftingRecipe> GetAllRecipes()
        {
            return allRecipes;
        }

        private void AddRecipe(RecipeComponent<ItemStack>[] inputs, ItemStack output)
        {
            allRecipes.Add(new CraftingRecipe(inputs, output, false));
        }

        private void AddMakeshiftRecipe(RecipeComponent<ItemStack>[] inputs, ItemStack output)
        {
            CraftingRecipe newRecipe = new CraftingRecipe(inputs, output, true);
            allRecipes.Add(newRecipe);
            makeshiftRecipes.Add(newRecipe);
        }

        public void LoadRecipes()
        {
            AddMakeshiftRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("ScrapMetal", 5),
                },
                new ItemStack("MakeshiftConduit", 10)
                );

            AddMakeshiftRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("MangledWires", 4),
                    new ItemStack("UsedBattery", 4),
                },
                new ItemStack("MakeshiftEnergyCell")
                );

            AddMakeshiftRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("MangledWires", 3),
                    new ItemStack("UsedBattery", 3),
                    new ItemStack("ScrapMetal", 7),
                },
                new ItemStack("MakeshiftRecycler")
                );

            AddMakeshiftRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("AluminumScrap", 2),
                    new ItemStack("SteelScrap", 2),
                },
                new ItemStack("Workbench")
                );

            AddRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("Lithium", 6),
                },
                new ItemStack("RechargableBattery")
                );

            AddRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("AluminumScrap", 2),
                    new ItemStack("SteelScrap", 2),
                },
                new ItemStack("Workbench")
                );

            AddRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("ShatteredGlass", 5),
                    new ItemStack("SpaceRock", 5),
                },
                new ItemStack("MakeshiftRefractory")
                );

            AddRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("Graphite", 6),
                },
                new ItemStack("GraphiteElectrode")
                );

            AddRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("AluminumScrap", 3),
                    new ItemStack("SteelScrap", 3),
                    new ItemStack("CopperScrap", 5),
                    new ItemStack("Workbench"),
                },
                new ItemStack("MakeshiftAssembler")
                );

            AddRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("AluminumScrap", 3),
                    new ItemStack("SteelScrap", 3),
                    new ItemStack("CopperScrap", 5),
                    new CategoryItemStack("RollingPass", 2),
                },
                new ItemStack("MakeshiftRollingMill")
                );

            AddRecipe(
                new RecipeComponent<ItemStack>[]
                {
                    new ItemStack("TitaniumScrap", 5),
                    new ItemStack("MakeshiftRefractory", 3),
                    new ItemStack("GraphiteElectrode", 3)
                },
                new ItemStack("MakeshiftArcFurnace")
                );
            // TODO continue
        }
    }
}
