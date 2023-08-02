using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

                if (!Recipe)
                {
                    itemStackDisplay.ItemStack = new ItemStack();
                }
                else
                {
                    itemStackDisplay.ItemStack = Recipe.Result;
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
