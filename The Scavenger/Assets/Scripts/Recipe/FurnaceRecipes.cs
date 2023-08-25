using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Recipes
{
    // TODO add docs
    public class FurnaceRecipes : RecipeList<FurnaceRecipe>
    {
        private static FurnaceRecipes instance = new FurnaceRecipes();
        public static FurnaceRecipes Instance => instance;

        private readonly List<FurnaceRecipe> recipes = new();

        public override List<FurnaceRecipe> GetRecipesWithInput(RecipeComponent input)
        {
            FurnaceRecipe recipe = GetRecipeWithInput(input);
            if (recipe == null)
            {
                return new();
            }
            return new() { recipe };
        }

        public FurnaceRecipe GetRecipeWithInput(RecipeComponent input)
        {
            if (input is not RecipeComponent<ItemStack>)
            {
                return null;
            }

            foreach (FurnaceRecipe recipe in recipes)
            {
                if (recipe.IsInput(input))
                {
                    return recipe;
                }
            }
            return null;
        }

        public override List<FurnaceRecipe> GetRecipesWithOutput(RecipeComponent output)
        {
            if (output is not RecipeComponent<ItemStack>)
            {
                return new();
            }
            return base.GetRecipesWithOutput(output);
        }

        public override List<FurnaceRecipe> GetRecipes() => recipes;
        public override List<Recipe> GetGenericRecipes()
        {
            return recipes.ConvertAll<Recipe>((furnaceRecipe) => furnaceRecipe);
        }


        private void AddRecipe(RecipeComponent<ItemStack> input, ItemStack output, int requiredTemp)
        {
            Debug.Assert(input.Amount == 1);
            Debug.Assert(output.Amount == 1);
            Debug.Assert(requiredTemp > 0);

            // One input cannot have multiple recipes
            foreach (FurnaceRecipe recipe in recipes)
            {
                Debug.Assert(!recipe.input.CanSubstituteWith(input));
            }

            recipes.Add(new FurnaceRecipe(input, output, null, requiredTemp));
        }

        private void AddRecipe(RecipeComponent<ItemStack> input, ItemStack output, ChanceItemStack byproduct, int requiredTemp)
        {
            Debug.Assert(input.Amount == 1);
            Debug.Assert(output.Amount == 1);
            Debug.Assert(byproduct.Amount == 1);
            Debug.Assert(requiredTemp > 0);
            recipes.Add(new FurnaceRecipe(input, output, byproduct, requiredTemp));
        }

        public override void LoadRecipes()
        {
            AddRecipe(
                new ItemStack("AluminumScrap"),
                new ItemStack("AluminumBloom"),
                new ChanceItemStack("Slag", 0.2f),
                1000
                );
            AddRecipe(
                new ItemStack("ShatteredGlass"),
                new ItemStack("GlassPane"),
                1500
                );
            AddRecipe(
                new ItemStack("GlassPane"),
                new ItemStack("SiliconShard"),
                1500
                );
            AddRecipe(
                new ItemStack("CopperScrap"),
                new ItemStack("CopperBloom"),
                new ChanceItemStack("Slag", 0.2f),
                2000
                );
            AddRecipe(
                new ItemStack("SteelScrap"),
                new ItemStack("SteelBloom"),
                new ChanceItemStack("Slag", 0.2f),
                2500
                );
            AddRecipe(
                new CategoryItemStack("RollingPass", 1),
                new ItemStack("SteelBloom"),
                new ChanceItemStack("Slag", 0.2f),
                2500
                );
            AddRecipe(
                new ItemStack("TitaniumScrap"),
                new ItemStack("TitaniumBloom"),
                new ChanceItemStack("Slag", 0.2f),
                3000
                );
            // TODO continue
        }
    }
}
