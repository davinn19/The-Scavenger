using Scavenger.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger
{
    [RequireComponent(typeof(ItemBuffer))]
    public class ItemSelection : MonoBehaviour
    {
        [SerializeField] private int selectedItem = 0;

        private ItemBuffer inventory;

        private void Awake()
        {
            inventory = GetComponent<ItemBuffer>();
        }

        private void Update()
        {
            float scrollDirection = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scrollDirection < 0)
            {
                selectedItem++;
                if (selectedItem >= InventoryUI.InventoryWidth)
                {
                    selectedItem = 0;
                }
            }
            else if (scrollDirection > 0)
            {
                selectedItem--;
                if (selectedItem < 0)
                {
                    selectedItem = InventoryUI.InventoryWidth - 1;
                }
            }
        }

        public ItemStack GetSelectedItemStack()
        {
            return inventory.GetItemInSlot(selectedItem);
        }

    }
}
