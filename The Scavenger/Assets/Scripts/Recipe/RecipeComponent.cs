using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public interface RecipeComponent
    {
        public abstract int Amount { get; }
        public abstract bool CanSubstituteWith(RecipeComponent other);
        public abstract string ToString();
    }

    public interface RecipeComponent<T> : RecipeComponent
    {
        public T GetRecipeComponent();
    }

    public interface ChanceRecipeComponent<T> : RecipeComponent<T>
    {
        public float GetChance();
    }

    public interface CategoryRecipeComponent<T> : RecipeComponent<T>
    {
        public string GetCategory();
    }

}
