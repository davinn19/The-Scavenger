using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Scavenger.UI
{
    // TODO add docs
    [RequireComponent(typeof(CategoryItemStackDisplay))]
    public class CategoryItemStackTooltipDefinition : ItemStackTooltipDefinition
    {
        private ItemStackDisplay itemDisplay;
        private CategoryItemStackDisplay categoryDisplay;

        private void Awake()
        {
            itemDisplay = GetComponent<ItemStackDisplay>();
            itemDisplay.ItemStackChanged += OnTargetChanged;

            categoryDisplay = GetComponent<CategoryItemStackDisplay>();

        }

        public override void RenderTooltip(Tooltip tooltip)
        {
            if (!itemDisplay.ItemStack)
            {
                return;
            }

            tooltip.AddHeader(new LocalizedString("Item Names", itemDisplay.ItemStack.Item.name));
            tooltip.AddText(new LocalizedString("Item Categories", categoryDisplay.Category.GetCategory()));
            // TODO add descrpition
        }
    }
}
