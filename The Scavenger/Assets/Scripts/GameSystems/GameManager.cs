using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Scavenger
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private GridMap map;
        [SerializeField] private SpriteRenderer tileHover;

        private Controls controls;
        private InputAction place;

        private ItemSelection itemSelection;

        private Vector3 mousePos;
        private Vector2Int gridPos;
        private GridObject gridObject;


        private void Awake()
        {
            itemSelection = GetComponent<ItemSelection>();
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

        private void Update()
        {
            ItemStack selectedItemStack = itemSelection.GetSelectedItemStack();

            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            gridPos = new Vector2Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));
            gridObject = map.GetObjectAtPos(gridPos);

            DrawPlacementPreview(selectedItemStack, gridPos);

            if (!selectedItemStack)
            {
                return;
            }
        }


        private void DrawPlacementPreview(ItemStack selectedItemStack, Vector2Int gridPos)
        {
            if (!selectedItemStack)
            {
                tileHover.enabled = false;
                return;
            }

            tileHover.enabled = true;
            tileHover.sprite = selectedItemStack.Item.Icon;
            tileHover.transform.position = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0);
        }


        public void OnPlace(InputAction.CallbackContext context)  // Left click to place/interact
        {
            // Only handle input if mouse is not over UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Ignore if not item is held
            ItemStack selectedItemStack = itemSelection.GetSelectedItemStack();
            if (!selectedItemStack)
            {
                return;
            }

            // Place Item
            if (!gridObject && selectedItemStack.Item.HasProperty<PlacedObject>())         // Clicked on empty space with placable object, place the object
            {
                map.TryPlaceItem(selectedItemStack.Item, gridPos);
            }
            else if (gridObject)    // Interact item
            {
                Vector2Int sidePressed = GetSidePressed(mousePos, tileHover.transform.position);
                map.TryInteract(selectedItemStack, gridPos, sidePressed);
            }
        }


        private Vector2Int GetSidePressed(Vector3 mousePos, Vector3 tilePos)
        {
            Vector3 relativePressedPos = mousePos - tilePos;
            Vector2Int greaterAxisPressed;
            int directionPressed;

            if (Mathf.Abs(relativePressedPos.x) > Mathf.Abs(relativePressedPos.y))
            {
                greaterAxisPressed = Vector2Int.right;
                directionPressed = Mathf.CeilToInt(Mathf.Sign(relativePressedPos.x));
            }
            else
            {
                greaterAxisPressed = Vector2Int.up;
                directionPressed = Mathf.CeilToInt(Mathf.Sign(relativePressedPos.y));
            }

            return greaterAxisPressed * directionPressed;
        }
    }
}
