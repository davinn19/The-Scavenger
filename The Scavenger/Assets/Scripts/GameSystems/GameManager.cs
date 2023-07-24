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
        public ItemBuffer Inventory { get; private set; }
        public ItemDropper ItemDropper { get; private set; }
        public InputMode InputMode { get; set; }

        private void Awake()
        {
            itemSelection = GetComponent<ItemSelection>();
            gridHover = GetComponent<GridHover>();
            Inventory = GetComponent<ItemBuffer>();
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

            // Ignore if no item is held
            ItemStack selectedItemStack = itemSelection.GetSelectedItemStack();
            if (!selectedItemStack)
            {
                return;
            }

            // Try placing object next
            bool placeSuccess = Map.TryPlaceItem(selectedItemStack, gridHover.HoveredPos);
            if (placeSuccess)
            {
                return;
            }

            // Try having grid object handle interaction next
            Vector2Int sidePressed = gridHover.GetHoveredSide();
            bool itemInteractSuccess = Map.TryObjectInteract(selectedItemStack, gridHover.HoveredPos, sidePressed);
            if (itemInteractSuccess)
            {
                return;
            }

            // Try having item handle interaction
            selectedItemStack.Item.Interact(this, itemSelection.SelectedSlot, gridHover.HoveredPos);
        }

    }
}
