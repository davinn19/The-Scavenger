using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUI : MonoBehaviour
    {
        public const int InventoryWidth = 7;
        public const int InventoryHeight = 4;

        [SerializeField] private ItemBuffer inventory;
        private ItemSlot[] itemSlots;

        [SerializeField] private Rect closedState;
        [SerializeField] private Rect openState;
        private RectTransform rectTransform;

        private bool inventoryOpen = false;

        private Controls controls;
        private InputAction toggleInventory;

        private void Awake()
        {
            itemSlots = GetComponentsInChildren<ItemSlot>();
            rectTransform = gameObject.GetComponent<RectTransform>();
            controls = new();
        }

        private void OnEnable()
        {
            toggleInventory = controls.GridMap.ToggleInventory;
            toggleInventory.Enable();
            toggleInventory.performed += OnInventoryToggle;
        }

        private void OnDisable()
        {
            toggleInventory.Disable();
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

        private void OnInventoryToggle(InputAction.CallbackContext context)
        {
            inventoryOpen = !inventoryOpen;
            if (inventoryOpen)
            {
                rectTransform.anchoredPosition = openState.position;
            }
            else
            {
                rectTransform.anchoredPosition = closedState.position;
            }
        }

        
    }
}
