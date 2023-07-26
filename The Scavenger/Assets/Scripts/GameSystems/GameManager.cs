using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public GridMap Map { get; private set; }

        private Controls controls;
        private InputAction place;

        private GridHover gridHover;
        private ItemSelection itemSelection;
        [field: SerializeField] public ItemBuffer Inventory { get; private set; }
        public ItemDropper ItemDropper { get; private set; }
        public InputMode InputMode { get; set; }

        private void Awake()
        {
            
            gridHover = GetComponent<GridHover>();
            itemSelection = GetComponent<ItemSelection>();
            ItemDropper = GetComponent<ItemDropper>();
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

            // Check for clickables first
            Clickable clickable = gridHover.GetClickableUnderMouse();
            if (clickable)
            {
                clickable.OnClick(this);
                return;
            }

            // Try placing object next
            bool placeSuccess = Map.TryPlaceItem(itemSelection, gridHover.HoveredPos);
            if (placeSuccess)
            {
                return;
            }

            // Try having grid object handle interaction next
            Vector2Int sidePressed = gridHover.GetHoveredSide();
            bool itemInteractSuccess = Map.TryObjectInteract(Inventory, itemSelection, gridHover.HoveredPos, sidePressed);
            if (itemInteractSuccess)
            {
                return;
            }

            // Try having item handle interaction
            ItemStack selectedItemStack = itemSelection.GetHeldItem();
            if (selectedItemStack)
            {
                selectedItemStack.Item.Interact(this, itemSelection, gridHover.HoveredPos);
            }
        }

    }
}
