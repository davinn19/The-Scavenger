using UnityEngine;

namespace Scavenger.UI.InspectorContent
{
    // TODO add docs
    public abstract class UIContent<T> : MonoBehaviour where T : class
    {
        protected T Target { get; set; }

        private void Awake()
        {
            enabled = false;
        }
    }
}
