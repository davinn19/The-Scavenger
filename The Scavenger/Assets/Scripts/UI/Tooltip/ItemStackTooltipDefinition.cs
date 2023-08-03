using Scavenger.UI.UIContent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    [RequireComponent(typeof(ItemStackDisplay))]
    public class ItemStackTooltipDefinition : TooltipDefinition
    {
        private ItemStackDisplay display;

        private void Awake()
        {
            display = GetComponent<ItemStackDisplay>();
            display.ItemStackChanged += OnTargetChanged;
        }

        public override bool IsVisible() => display.ItemStack;

        // expected to create an activated copy
        public override GameObject GetCustomContent()
        {
            if (!display.ItemStack)
            {
                return null;
            }

            Item item = display.ItemStack.Item;

            CustomUI customUI;
            if (!item.TryGetProperty(out customUI))
            {
                return null;
            }

            ItemUIContent customTooltip = Instantiate(customUI.UI);
            customTooltip.Init(item);

            return customTooltip.gameObject;
        }

        public override string GetDescription()
        {
            if (!display.ItemStack)
            {
                return "";
            }

            return display.ItemStack.Item.Description;
        }

        public override string GetHeader()
        {
            if (!display.ItemStack)
            {
                return "";
            }

            return display.ItemStack.Item.DisplayName;
        }
    }
}
