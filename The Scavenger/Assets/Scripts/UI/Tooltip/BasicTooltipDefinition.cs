using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Scavenger.UI
{
    // TODO add docs
    public class BasicTooltipDefinition : TooltipDefinition
    {
        [SerializeField] private LocalizedString header;
        [SerializeField] private LocalizedString description;

        public override void RenderTooltip(Tooltip tooltip)
        {
            tooltip.AddHeader(header);

            if (description != null)
            {
                tooltip.AddText(description);
            }
        }
    }
}
