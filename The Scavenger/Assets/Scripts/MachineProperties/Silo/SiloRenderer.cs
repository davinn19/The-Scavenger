using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Changes the gridObject's appearance based on the silo's contents.
    /// </summary>
    [RequireComponent(typeof(ItemBuffer))]
    public class SiloRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer icon;
        private ItemBuffer buffer;

        private void Awake()
        {
            buffer = GetComponent<ItemBuffer>();

            GridObject gridObject = GetComponent<GridObject>();
            gridObject.SelfChanged += UpdateAppearance;
            buffer.SlotChanged += (_) => UpdateAppearance();
        }

        /// <summary>
        /// Changes the gridObject's appearance based on the silo's contents.
        /// </summary>
        private void UpdateAppearance()
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
