using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scavenger.GridObjectBehaviors;
using Scavenger.Recipes;

namespace Scavenger.UI.InspectorContent
{
    // TODO implement, add docs, make updates when the player's inventory changes
    public class CraftingUI : GridObjectInspectorContent
    {
        private TMP_InputField searchBar;
        private Crafter.FilterMode filterMode = Crafter.FilterMode.All;

        private Crafter crafter;

        private CraftingRecipe m_currentRecipe;
        private CraftingRecipe currentRecipe
        {
            get
            {
                return m_currentRecipe;
            }
            set
            {
                m_currentRecipe = value;
                UpdateCurrentRecipe();
            }
        }

        [SerializeField] private RecipeResultDisplay recipeResultDisplayPrefab;

        [SerializeField] private RectTransform recipeSearchResults;

        [SerializeField] private ItemStackDisplay resultDisplay;
        [SerializeField] private RectTransform ingredientDisplaysGroup;
        private CategoryItemStackDisplay[] ingredientDisplays;


        private void Awake()
        {
            searchBar = GetComponentInChildren<TMP_InputField>();
            ingredientDisplays = ingredientDisplaysGroup.GetComponentsInChildren<CategoryItemStackDisplay>();
        }

        public override void Init(GridObject target, PlayerInventory inventory)
        {
            base.Init(target, inventory);
            crafter = target.GetComponent<Crafter>();
            currentRecipe = null;
            searchBar.onValueChanged.AddListener(UpdateRecipeSearch);
            UpdateRecipeSearch(searchBar.text);
        }


        // TODO add docs
        private void UpdateRecipeSearch(string searchText)
        {
            List<CraftingRecipe> recipes = crafter.GetRecipes(searchText, filterMode, Inventory);
            RecipeResultDisplay[] displays = recipeSearchResults.GetComponentsInChildren<RecipeResultDisplay>();

            // Override existing item displays and add more if necessary
            for (int i = 0; i < recipes.Count; i++)
            {
                CraftingRecipe recipe = recipes[i];

                RecipeResultDisplay display;
                if (i < displays.Length)
                {
                    display = displays[i];
                    display.gameObject.SetActive(true);
                }
                else
                {
                    display = CreateRecipeResultDisplay(recipe);
                }

                display.Recipe = recipe;
            }

            // Disable unused displays
            for (int i = recipes.Count; i < displays.Length; i++)
            {
                displays[i].gameObject.SetActive(false);
            }
        }

        // TODO add docs
        private RecipeResultDisplay CreateRecipeResultDisplay(CraftingRecipe recipe)
        {
            RecipeResultDisplay display = Instantiate(recipeResultDisplayPrefab);
            display.Recipe = recipe;
            display.DisplayPressed += OnSearchResultPressed;

            display.transform.SetParent(recipeSearchResults, false);

            return display;
        }

        // TODO add docs
        private void OnSearchResultPressed(RecipeResultDisplay resultPressed)
        {
            currentRecipe = resultPressed.Recipe;
        }

        // TODO add docs, implement
        private void UpdateCurrentRecipe()
        {
            // Clear current recipe displayed
            foreach (CategoryItemStackDisplay ingredientDisplay in ingredientDisplays)
            {
                ingredientDisplay.Clear();
            }
            resultDisplay.ItemStack = new ItemStack();


            // If there is no recipe selected, finish here
            if (currentRecipe == null)
            {
                return;
            }

            // Fill in display with the new recipe
            for (int i = 0; i < currentRecipe.Inputs.Length; i++)
            {
                RecipeComponent<ItemStack> ingredient = currentRecipe.Inputs[i];    // TODO add rotating slot
                CategoryItemStackDisplay ingredientDisplay = ingredientDisplays[i];
                if (ingredient is not CategoryItemStack)
                {
                    ingredientDisplay.SetItemStack(ingredient.GetRecipeComponent());
                }
                else
                {
                    ingredientDisplay.Category = ingredient as CategoryItemStack;
                }

            }
            resultDisplay.ItemStack = currentRecipe.Output;
        }

        // TODO add docs
        public void CraftOne() => crafter.Craft(currentRecipe, 1, Inventory);

        // TODO add docs
        public void CraftStack() => crafter.Craft(currentRecipe, currentRecipe.Output.MaxAmount, Inventory);

        // TODO add docs
        public void CraftMax() => crafter.Craft(currentRecipe, int.MaxValue, Inventory);

        // TODO add docs
        public void RotateFilterMode()
        {
            filterMode++;
            if ((int)filterMode > 2)
            {
                filterMode = 0;
            }
        }
    }
}
