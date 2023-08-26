using Scavenger.GridObjectBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    // TODO implement, add docs, redesign this and slotdisplays
    [RequireComponent(typeof(ItemStackDisplay))]
    public class FilteredDisplay : MonoBehaviour
    {
        [SerializeField] private Image filterImage;
        [SerializeField] private Image itemImage;

        private ItemStackDisplay itemStackDisplay;
        public ItemStack Filter { get; private set; }

        private void Awake()
        {
            itemStackDisplay = GetComponent<ItemStackDisplay>();
            itemStackDisplay.AppearanceChanged += UpdateDisplay;
        }

        private void OnEnable()
        {
            if (Filter != null)
            {
                Filter.Changed += UpdateDisplay;
            }
        }

        private void OnDisable()
        {
            if (Filter != null)
            {
                Filter.Changed -= UpdateDisplay;
            }
        }

        public void SetFilter(ItemStack filter)
        {
            Debug.Assert(filter != null);

            if (Filter == filter)
            {
                return;
            }

            if (Filter != null)
            {
                Filter.Changed -= UpdateDisplay;
            }

            Filter = filter;
            Filter.Changed += UpdateDisplay;
        }

        private void UpdateDisplay()
        {
            if (Filter == null || !Filter || itemImage.enabled)
            {
                filterImage.enabled = false;
            }
            else
            {
                filterImage.enabled = true;
                filterImage.sprite = Filter.Item.Icon;
            }
        }
    }
}
