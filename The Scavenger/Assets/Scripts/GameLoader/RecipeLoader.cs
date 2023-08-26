using Scavenger.Recipes;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class RecipeLoader : MonoBehaviour
    {
        private RecipeList[] recipes;

        public void LoadRecipes()
        {
            recipes = new RecipeList[]
            {
                RecyclerRecipes.Instance,
                CraftingRecipes.Instance,
                FurnaceRecipes.Instance,
                RollingRecipes.Instance,
                AssemblerRecipes.Instance,
            };

            foreach (RecipeList recipeList in recipes)
            {
                recipeList.LoadRecipes();
            }
        }
    }
}
