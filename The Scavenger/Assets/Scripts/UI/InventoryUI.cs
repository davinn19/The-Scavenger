using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        private List<ItemSlot> itemSlots;

        private void Awake()
        {
            itemSlots = new List<ItemSlot>(GetComponentsInChildren<ItemSlot>());
        }

        private void Update()
        {
            for (int y = 0; y < Inventory.InventoryHeight; y++)
            {
                for (int x = 0; x < Inventory.InventoryWidth; x++)
                {
                    ItemSlot slot = GetItemSlot(x, y);
                    ItemStack itemStack = inventory.GetItemStack(x, y);

                    slot.itemStack = itemStack;
                }
            }
        }

        private ItemSlot GetItemSlot(int x, int y)
        {
            return itemSlots[x + y * Inventory.InventoryWidth];
        }
    }
}
