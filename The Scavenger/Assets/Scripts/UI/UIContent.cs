using UnityEngine;

namespace Scavenger.UI.UIContent
{
    // TODO add docs
    public abstract class UIContent<T> : MonoBehaviour where T : class
    {
        protected T target { get; set; }

        private void Awake()
        {
            enabled = false;
        }

        public virtual void Init(T target)
        {
            this.target = target;
            enabled = true;
        }
    }
}
