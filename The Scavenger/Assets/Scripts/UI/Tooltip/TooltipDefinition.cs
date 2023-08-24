using System;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    public abstract class TooltipDefinition : MonoBehaviour
    {
        public event Action TargetChanged;

        protected void OnTargetChanged()
        {
            TargetChanged?.Invoke();
        }

        public abstract void RenderTooltip(Tooltip tooltip);
        public virtual void ClearTooltip(Tooltip tooltip) => tooltip.Clear();
    }
}
