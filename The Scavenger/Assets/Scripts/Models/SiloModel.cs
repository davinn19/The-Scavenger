using Scavenger.GridObjectBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Models
{
    // TODO add docs
    public class SiloModel : Model
    {
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private SpriteRenderer lockIcon;
        private SiloItemBuffer buffer;

        public override void Load(GridObjectBehavior behavior)
        {
            buffer = behavior.GetComponent<SiloItemBuffer>();
        }

        /// <summary>
        /// Changes the gridObject's appearance based on the silo's contents.
        /// </summary>
        public override void UpdateAppearance()
        {
            ItemStack itemStack = buffer.GetItemInSlot(0);

            lockIcon.enabled = buffer.IsLocked();

            if (itemStack)
            {
                icon.sprite = itemStack.Item.Icon;
            }
            else if (buffer.lockFilter)
            {
                icon.sprite = buffer.lockFilter.Item.Icon;
            }
            else
            {
                icon.sprite = null;
            }
        }
    }
}
