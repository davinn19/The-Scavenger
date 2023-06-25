using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [CreateAssetMenu(fileName = "RecyclerRecipes", menuName = "Scavenger/Recipes/Recycler")]
    public class RecyclerRecipes : ScriptableObject
    {
        [SerializeField] private List<RecyclerRecipe> recipes;
        

        public bool IsRecyclable(ItemStack item)
        {
            return GetRecipe(item.item) != null;
        }


        public RecyclerRecipe GetRecipe(Item input)
        {
            foreach (RecyclerRecipe recipe in recipes)
            {
                if (recipe.input == input)
                {
                    return recipe;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class RecyclerRecipe
    {
        public Item input;
        public List<DropEntry> output;
    }

    [System.Serializable]
    public class DropEntry
    {
        public Item item;
        public int amount;
        public float weight;
    }
}
