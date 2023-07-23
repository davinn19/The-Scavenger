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
        public InputMode InputMode { get; set; }

        private void Awake()
        {
            itemSelection = GetComponent<ItemSelection>();
            gridHover = GetComponent<GridHover>();
            Inventory = GetComponent<ItemBuffer>();
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
