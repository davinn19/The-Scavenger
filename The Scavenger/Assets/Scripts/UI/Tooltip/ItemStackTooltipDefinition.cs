using Scavenger.UI.InspectorContent;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

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
            display.AppearanceChanged += OnTargetChanged;
        }

        public override void RenderTooltip(Tooltip tooltip)
        {
            if (!display.ItemStack)
            {
                return;
            }

            tooltip.AddHeader(new LocalizedString("Item Names", display.ItemStack.Item.name));
            // TODO add descrpition
        }
    }
}
