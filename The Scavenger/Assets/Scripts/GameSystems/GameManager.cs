using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Scavenger
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridMap map;

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


        public void OnPlace(InputAction.CallbackContext context)  // Left click to place/interact
        {
            // Only handle input if mouse is not over UI
            if (gridHover.OverGUI)
            {
                return;
            }

            // Ignore if not item is held
            ItemStack selectedItemStack = itemSelection.GetSelectedItemStack();
            if (!selectedItemStack)
            {
                return;
            }

            GridObject gridObject = map.GetObjectAtPos(gridHover.HoveredPos);

            // Place Item
            if (!gridObject && selectedItemStack.Item.HasProperty<PlacedObject>())         // Clicked on empty space with placable object, place the object
            {
                map.TryPlaceItem(selectedItemStack.Item, gridHover.HoveredPos);
            }
            else if (gridObject)    // Interact item
            {
                Vector2Int sidePressed = gridHover.GetHoveredSide();
                map.TryInteract(selectedItemStack, gridHover.HoveredPos, sidePressed);
            }
        }


        
    }
}
