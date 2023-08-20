using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public interface RecipeList
    {
        public abstract void LoadRecipes();
        public abstract List<Recipe> GetRecipes();
    }

    // TODO implement, add docs
    public interface RecipeList<T> : RecipeList where T : Recipe
    {
        public abstract List<T> GetRecipesWithOutput(RecipeComponent output);
        public abstract List<T> GetRecipesWithInput(RecipeComponent input);
    }
}
