using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scavenger.UI.UIContent
{
    // TODO implement, add docs
    public class CraftingUI : GridObjectUIContent
    {
        private Crafter crafter;
        private TMP_InputField searchBar;

        private void Awake()
        {
            searchBar = GetComponentInChildren<TMP_InputField>();
        }

        public override void Init(GridObject gridObject)
        {
            base.Init(gridObject);

            crafter = gridObject.GetComponent<Crafter>();
        }

        public void UpdateRecipeSearch(string searchInput)
        {
            // TODO implement
        }

    }
}
