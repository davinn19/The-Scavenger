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
        private ItemBuffer buffer;

        public override void Load(GridObjectBehavior behavior)
        {
            buffer = behavior.GetComponent<ItemBuffer>();

            // TODO figure out where self changed update should go
            behavior.GetComponent<GridObject>().SelfChanged += UpdateAppearance;
            buffer.SlotChanged += (_) => UpdateAppearance();
        }

        /// <summary>
        /// Changes the gridObject's appearance based on the silo's contents.
        /// </summary>
        public override void UpdateAppearance()
        {
            ItemStack itemStack = buffer.GetItemInSlot(0);

            if (!itemStack)
            {
                icon.sprite = null;
            }
            else
            {
                icon.sprite = itemStack.Item.Icon;
            }
        }
    }
}
