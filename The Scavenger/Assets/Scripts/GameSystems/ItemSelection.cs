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
        public int selectedItem { get; private set; }

        private ItemBuffer inventory;

        private Controls controls;
        private InputAction selectItem;

        private void Awake()
        {
            inventory = GetComponent<ItemBuffer>();
            controls = new Controls();
        }

        private void OnEnable()
        {
            selectItem = controls.GridMap.SelectItem;
            selectItem.Enable();
            selectItem.performed += UpdateSelectedItem;
        }

        private void UpdateSelectedItem(InputAction.CallbackContext context)
        {
            float scrollDirection = context.ReadValue<float>();

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
