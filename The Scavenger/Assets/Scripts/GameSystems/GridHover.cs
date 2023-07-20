using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Scavenger
{
    public class GridHover : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer hoverIndicator;

        
        public Vector2Int HoveredPos { get; private set; }
        public bool OverGUI { get; private set; }
        private Vector2 mousePos;

        private Controls controls;
        private InputAction pointerHover;

        private ItemSelection itemSelection;

        private void Awake()
        {
            controls = new Controls();
            itemSelection = GetComponent<ItemSelection>();
        }

        private void OnEnable()
        {
            pointerHover = controls.GridMap.PointerHover;
            pointerHover.Enable();
        }

        private void OnDisable()
        {
            pointerHover.Disable();
        }


        private void Update()
        {
            OverGUI = EventSystem.current.IsPointerOverGameObject();

            mousePos = Camera.main.ScreenToWorldPoint(pointerHover.ReadValue<Vector2>());
            HoveredPos = new Vector2Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));

            DrawHoverIndicator();
        
        }

        private void DrawHoverIndicator()
        {
            ItemStack selectedItemStack = itemSelection.GetSelectedItemStack();

            if (OverGUI || !selectedItemStack)
            {
                hoverIndicator.enabled = false;
                return;
            }

            hoverIndicator.enabled = true;
            hoverIndicator.sprite = selectedItemStack.Item.Icon;
            hoverIndicator.transform.position = GridMap.GetCenterOfTile(HoveredPos);
        }


        public Vector2Int GetHoveredSide()
        {
            Vector2 relativePressedPos = mousePos - GridMap.GetCenterOfTile(HoveredPos);
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
