using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public GridMap Map { get; private set; }

        private Controls controls;
        private InputAction place;

        private ItemSelection itemSelection;
        private GridHover gridHover;


        private void Awake()
        {
            itemSelection = GetComponent<ItemSelection>();
            gridHover = GetComponent<GridHover>();
            controls = new();
        }

        private void OnEnable()
        {
            place = controls.GridMap.PlaceInteractItem;
            place.Enable();
            place.performed += OnPlace;

        }

        private void OnDisable()
        {
            place.Disable();
        }        

        private void OnPlace(InputAction.CallbackContext context)  // Left click to place/interact
        {
            // Only handle input if mouse is not over UI
            if (gridHover.OverGUI)
            {
                return;
            }

            // Ignore if no item is held
            ItemStack selectedItemStack = itemSelection.GetSelectedItemStack();
            if (!selectedItemStack)
            {
                return;
            }

            GridObject gridObject = Map.GetObjectAtPos(gridHover.HoveredPos);

            // Place Item
            if (!gridObject)         // Clicked on empty space with placable object, place the object
            {
                Map.TryPlaceItem(selectedItemStack, gridHover.HoveredPos);
            }
            else                     // Interact item
            {
                Vector2Int sidePressed = gridHover.GetHoveredSide();
                Map.TryInteract(selectedItemStack, gridHover.HoveredPos, sidePressed);
            }
        }

        
    }
}
