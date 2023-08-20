using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class FurnaceRecipes : RecipeList<FurnaceRecipe>
    {
        private static FurnaceRecipes instance = new FurnaceRecipes();
        public static FurnaceRecipes Instance => instance;

        private readonly List<FurnaceRecipe> recipes = new();

        public List<FurnaceRecipe> GetRecipesWithInput(RecipeComponent input)
        {
            List<FurnaceRecipe> recipesWithInput = new();
            foreach (FurnaceRecipe recipe in recipes)
            {
                if (recipe.IsInput(input))
                {
                    recipesWithInput.Add(recipe);
                }
            }
            return recipesWithInput;
        }

        public List<FurnaceRecipe> GetRecipesWithOutput(RecipeComponent output)
        {
            List<FurnaceRecipe> recipesWithOutput = new();
            foreach (FurnaceRecipe recipe in recipes)
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
            return recipes.ConvertAll<Recipe>((furnaceRecipe) => furnaceRecipe);
        }

        public void LoadRecipes()
        {
            // TODO continue
        }
    }
}
