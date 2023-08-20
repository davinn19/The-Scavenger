using System;
using System.Collections;
using System.Collections.Generic;
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
                FurnaceRecipes.Instance
            };

            foreach (RecipeList recipeList in recipes)
            {
                recipeList.LoadRecipes();
            }
        }
    }
}
