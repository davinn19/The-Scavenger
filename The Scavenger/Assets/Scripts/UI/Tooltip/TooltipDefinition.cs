using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    public abstract class TooltipDefinition : MonoBehaviour
    {
        public event Action TargetChanged;
        public abstract string GetHeader();
        public abstract string GetDescription();
        public abstract GameObject GetCustomContent();

        protected void OnTargetChanged()
        {
            TargetChanged?.Invoke();
        }
    }
}
