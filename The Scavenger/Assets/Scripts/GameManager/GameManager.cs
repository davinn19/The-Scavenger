using System.Collections.Generic;
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
        [field: SerializeField] public GridMap Map { get; private set; }
        public PlayerInventory Inventory { get; private set; }
        public InputHandler InputHandler { get; private set; }
        public ItemDropper ItemDropper { get; private set; }
        public GridObjectInspector GridObjectInspector { get; private set; }
        

        private void Awake()
        {
            ItemDropper = GetComponent<ItemDropper>();
            Inventory = GetComponent<PlayerInventory>();
            GridObjectInspector = GetComponent<GridObjectInspector>();

            InputHandler = GetComponent<InputHandler>();
            InputHandler.PointerClicked += OnPointerClick;
        }


        // TODO add docs
        private void OnPointerClick(InputMode inputMode)
        {
            Vector2Int gridPos = InputHandler.HoveredGridPos;
            Vector2Int sidePressed = InputHandler.GetHoveredSide();

            // Try interacting clickable first
            if (TryInteractClickable())
            {
                return;
            }

            switch (inputMode)
            {
                case InputMode.Interact:
                    

                    // Try placing object next
                    if (Map.TryPlaceItem(Inventory, gridPos))
                    {
                        return;
                    }

                    // Try having grid object handle interaction next
                    if (Map.TryObjectInteract(Inventory, gridPos, sidePressed))
                    {
                        return;
                    }

                    // Try having item handle interaction
                    ItemStack selectedItemStack = Inventory.GetHeldItem();
                    if (selectedItemStack)
                    {
                        selectedItemStack.Item.Interact(this, gridPos);
                    }
                    break;

                case InputMode.Edit:
                    Map.TryEdit(gridPos, sidePressed);
                    break;

                case InputMode.Remove:
                    List<ItemStack> droppedItems = Map.RemoveAtPos(gridPos);
                    foreach (ItemStack droppedItem in droppedItems)
                    {
                        ItemDropper.CreateFloatingItem(droppedItem, GridMap.GetCenterOfTile(gridPos));
                    }
                    break;
            }
        }

        // TODO add docs
        private bool TryInteractClickable()
        {
            Clickable clickable = InputHandler.GetClickableUnderPointer();
            if (clickable)
            {
                clickable.OnClick(this);
            }

            return clickable;
        }
    }
}
