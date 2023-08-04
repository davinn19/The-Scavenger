using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scavenger.GridObjectBehaviors;    

namespace Scavenger.UI.UIContent
{
    // TODO implement, add docs, make updates when the player's inventory changes
    public class CraftingUI : GridObjectUIContent
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
        private ItemStackDisplay[] ingredientDisplays;


        private void Awake()
        {
            searchBar = GetComponentInChildren<TMP_InputField>();
            ingredientDisplays = ingredientDisplaysGroup.GetComponentsInChildren<ItemStackDisplay>();

            currentRecipe = null;
        }

        public override void Init(GridObject target, ItemBuffer inventory)
        {
            base.Init(target, inventory);
            crafter = target.GetComponent<Crafter>();
        }

        private void Update()
        {
            UpdateRecipeSearch();
        }

        // TODO add docs
        private void UpdateRecipeSearch()
        {
            // TODO implement
            List<CraftingRecipe> recipes = crafter.GetRecipes(searchBar.text, filterMode, Inventory);
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
            foreach (ItemStackDisplay ingredientDisplay in ingredientDisplays)
            {
                ingredientDisplay.ItemStack = new ItemStack();
            }
            resultDisplay.ItemStack = new ItemStack();


            // If there is no recipe selected, finish here
            if (!currentRecipe)
            {
                return;
            }

            // Fill in display with the new recipe
            for (int i = 0; i < currentRecipe.Ingredients.Count; i++)
            {
                ItemStack ingredient = currentRecipe.Ingredients[i];
                ItemStackDisplay ingredientDisplay = ingredientDisplays[i];
                ingredientDisplay.ItemStack = ingredient;
            }
            resultDisplay.ItemStack = currentRecipe.Result;
        }

        // TODO add docs
        public void CraftOne() => crafter.Craft(currentRecipe, 1, Inventory);

        // TODO add docs
        public void CraftStack() => crafter.Craft(currentRecipe, 99, Inventory);// TODO finalize stack amount

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
