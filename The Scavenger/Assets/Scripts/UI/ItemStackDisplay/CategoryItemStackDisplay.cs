using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    [RequireComponent(typeof(ItemStackDisplay))]
    public class CategoryItemStackDisplay : MonoBehaviour
    {
        private int itemIndex = 0;

        private CategoryItemStack m_category;
        public CategoryItemStack Category
        {
            get => m_category;
            set
            {
                m_category = value;
                if (m_category != null)
                {
                    itemsInCategory = ItemDatabase.GetItemsInCategory(Category.GetCategory());
                    SwitchDisplayedItem();
                }
            }
        }

        private List<Item> itemsInCategory = new();

        private ItemStackDisplay itemStackDisplay;

        private void Awake()
        {
            itemStackDisplay = GetComponent<ItemStackDisplay>();
            GetComponentInParent<CategoryItemStackDisplayTimer>().SwitchTime += SwitchDisplayedItem;
        }

        public void SetItemStack(ItemStack itemStack)
        {
            Clear();
            itemStackDisplay.ItemStack = itemStack;
        }

        public void Clear()
        {
            itemStackDisplay.ItemStack = new ItemStack();
            Category = null;
            itemsInCategory = new();
        }

        private void SwitchDisplayedItem()
        {
            itemIndex++;

            if (itemsInCategory.Count == 0)
            {
                return;
            }

            itemIndex %= itemsInCategory.Count;
            itemStackDisplay.ItemStack = new ItemStack(itemsInCategory[itemIndex], Category.Amount);
        }
    }
}
