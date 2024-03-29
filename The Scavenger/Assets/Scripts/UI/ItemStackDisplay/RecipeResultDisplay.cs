using Scavenger.Recipes;
using System;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    [RequireComponent(typeof(ItemStackDisplay))]
    public class RecipeResultDisplay : MonoBehaviour
    {
        private ItemStackDisplay itemStackDisplay;

        public event Action<RecipeResultDisplay> DisplayPressed;

        private CraftingRecipe m_recipe;
        public CraftingRecipe Recipe
        {
            get
            {
                return m_recipe;
            }
            set
            {
                m_recipe = value;

                if (Recipe == null)
                {
                    itemStackDisplay.ItemStack = new ItemStack();
                }
                else
                {
                    itemStackDisplay.ItemStack = Recipe.Output;
                }
            }
        }

        private void Awake()
        {
            itemStackDisplay = GetComponent<ItemStackDisplay>();
        }

        public void OnPointerClick()
        {
            DisplayPressed?.Invoke(this);
        }

    }
}
