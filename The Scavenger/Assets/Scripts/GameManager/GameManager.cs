using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger
{
    /// <summary>
    /// Handles broader game logic.
    /// </summary>
    public class GameManager : MonoBehaviour    // TODO add docs
    {
        [field: SerializeField] public GameObject Player;
        [field: SerializeField] public ItemBuffer Inventory { get; private set; }
        [field: SerializeField] public GridMap Map { get; private set; }
        public HeldItemHandler HeldItemHandler { get; private set; }
        public InputHandler InputHandler { get; private set; }
        public ItemDropper ItemDropper { get; private set; }
        public GridObjectInspector GridObjectInspector { get; private set; }
        

        private void Awake()
        {
            ItemDropper = GetComponent<ItemDropper>();
            HeldItemHandler = GetComponentInChildren<HeldItemHandler>();
            GridObjectInspector = GetComponent<GridObjectInspector>();

            InputHandler = GetComponent<InputHandler>();
            InputHandler.PointerClicked += OnPointerClick;
        }


        // TODO add docs
        private void OnPointerClick(InputMode inputMode)
        {
            Vector2Int gridPos = InputHandler.HoveredGridPos;
            Vector2Int sidePressed = InputHandler.GetHoveredSide();

            switch (inputMode)
            {
                case InputMode.Interact:
                    // Try placing object first
                    if (Map.TryPlaceItem(HeldItemHandler, gridPos))
                    {
                        return;
                    }

                    // Try having grid object handle interaction next
                    if (Map.TryObjectInteract(Inventory, HeldItemHandler, gridPos, sidePressed))
                    {
                        return;
                    }

                    // Try having item handle interaction
                    ItemStack selectedItemStack = HeldItemHandler.GetHeldItem();
                    if (selectedItemStack)
                    {
                        selectedItemStack.Item.Interact(this, HeldItemHandler, gridPos);
                    }
                    break;

                case InputMode.Edit:
                    Map.TryEdit(gridPos, sidePressed);
                    break;

                case InputMode.Remove:
                    // TODO implement
                    break;
                case InputMode.Target:
                    Clickable clickable = InputHandler.GetClickableUnderPointer();
                    if (clickable)
                    {
                        clickable.OnClick(this);
                    }
                    break;
            }
        }
    }
}
