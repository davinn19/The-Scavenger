using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public const int InventoryWidth = 7;
        public const int InventoryHeight = 4;

        [SerializeField] private ItemBuffer inventory;
        private ItemSlot[] itemSlots;

        private void Awake()
        {
            itemSlots = GetComponentsInChildren<ItemSlot>();
        }

        private void Update()
        {
            for (int slotIndex = 0; slotIndex < itemSlots.Length; slotIndex++)
            {
                ItemSlot slotDisplay = itemSlots[slotIndex];
                ItemStack itemStack = inventory.GetItemInSlot(slotIndex);
                slotDisplay.itemStack = itemStack;
            }
        }
    }
}
