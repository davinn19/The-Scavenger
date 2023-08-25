using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Recipes
{
    public abstract class RecipeList
    {
        public abstract void LoadRecipes();
        public abstract List<Recipe> GetGenericRecipes();
    }

    // TODO implement, add docs
    public abstract class RecipeList<T> : RecipeList where T : Recipe
    {
        public abstract List<T> GetRecipes();

        public virtual List<T> GetRecipesWithOutput(RecipeComponent output)
        {
            List<T> recipes = new List<T>();
            foreach (T recipe in GetRecipes())
            {
                if (recipe.IsOutput(output))
                {
                    recipes.Add(recipe);
                }
            }
            return recipes;
        }

        public virtual List<T> GetRecipesWithInput(RecipeComponent input)
        {
            List<T> recipes = new List<T>();
            foreach (T recipe in GetRecipes())
            {
                if (recipe.IsInput(input))
                {
                    recipes.Add(recipe);
                }
            }
            return recipes;
        }
    }
}
