using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    public class BasicTooltipDefinition : TooltipDefinition
    {
        [SerializeField] private string header;
        [SerializeField, Multiline] private string description;
        [SerializeField] private GameObject customContent;

        public override bool IsVisible() => true;

        public override GameObject GetCustomContent()
        {
            return customContent;
        }

        public override string GetDescription()
        {
            return description;
        }

        public override string GetHeader()
        {
            return header;
        }
    }
}
