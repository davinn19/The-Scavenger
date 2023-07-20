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

        [SerializeField] private ItemSelection itemSelection;
        [SerializeField] private ItemBuffer inventory;
        private SlotDisplay[] slotDisplays;

        [SerializeField] private Rect closedState;
        [SerializeField] private Rect openState;
        private RectTransform rectTransform;

        [SerializeField] private RectTransform selectedItemIndicator;

        private bool inventoryOpen = false;

        private Controls controls;
        private InputAction toggleInventory;

        private void Awake()
        {
            InitSlotDisplays();

            rectTransform = gameObject.GetComponent<RectTransform>();
            controls = new();

            
        }

        private void InitSlotDisplays()
        {
            slotDisplays = GetComponentsInChildren<SlotDisplay>();

            for (int slot = 0; slot < slotDisplays.Length; slot++)
            {
                SlotDisplay slotDisplay = slotDisplays[slot];
                slotDisplay.buffer = inventory;
                slotDisplay.slot = slot;
            }
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
            // TODO link to event
            selectedItemIndicator.anchoredPosition = new Vector2(itemSelection.selectedItem * 55, 0);
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
