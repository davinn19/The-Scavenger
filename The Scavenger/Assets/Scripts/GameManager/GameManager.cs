using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger
{
    /// <summary>
    /// Handles broader game logic.
    /// </summary>
    public class GameManager : MonoBehaviour    // TODO add docs
    {
        [field: SerializeField] public GridMap Map { get; private set; }

        private Controls controls;
        private InputAction place;

        public HeldItemHandler HeldItemHandler { get; private set; }
        public InputHandler InputHandler { get; private set; }
        public ItemDropper ItemDropper { get; private set; }
        [field: SerializeField] public ItemBuffer Inventory { get; private set; }


        private void Awake()
        {
            ItemDropper = GetComponent<ItemDropper>();
            InputHandler = GetComponent<InputHandler>();
            HeldItemHandler = GetComponentInChildren<HeldItemHandler>();

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

        // TODO move to input handler
        private void OnPlace(InputAction.CallbackContext context)  // Left click to place/interact
        {
            // Only handle input if mouse is not over UI
            if (InputHandler.OverGUI)
            {
                return;
            }

            // Check for clickables first
            Clickable clickable = InputHandler.GetClickableUnderPointer();
            if (clickable)
            {
                clickable.OnClick(this);
                return;
            }

            // Try placing object next
            bool placeSuccess = Map.TryPlaceItem(HeldItemHandler, InputHandler.HoveredGridPos);
            if (placeSuccess)
            {
                return;
            }

            // Try having grid object handle interaction next
            Vector2Int sidePressed = InputHandler.GetHoveredSide();
            bool itemInteractSuccess = Map.TryObjectInteract(Inventory, HeldItemHandler, InputHandler.HoveredGridPos, sidePressed);
            if (itemInteractSuccess)
            {
                return;
            }

            // Try having item handle interaction
            ItemStack selectedItemStack = HeldItemHandler.GetHeldItem();
            if (selectedItemStack)
            {
                selectedItemStack.Item.Interact(this, HeldItemHandler, InputHandler.HoveredGridPos);
            }
        }

    }
}
