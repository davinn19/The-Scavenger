using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Scavenger
{
    /// <summary>
    /// Handles player input and event calls.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        public bool OverGUI { get; private set; }

        public event Action PointerMoved;
        public event Action<InputMode> PointerClicked; 
        public Vector2Int HoveredGridPos { get; private set; }
        public Vector2 HoveredWorldPos { get; private set; }

        public event Action<InputMode> InputModeChanged;
        private InputMode m_inputMode;
        public InputMode InputMode
        {
            get
            {
                return m_inputMode;
            }
            set
            {
                if (m_inputMode != value)
                {
                    m_inputMode = value;
                    InputModeChanged.Invoke(InputMode);
                }
            }
        }


        private Controls controls;
        private InputAction pointerHover;
        private InputAction pointerPressed;

        private void Awake()
        {
            controls = new Controls();
        }

        private void OnEnable()
        {
            pointerHover = controls.GridMap.PointerHover;
            pointerHover.Enable();
            pointerHover.performed += UpdateHoveredPos;

            pointerPressed = controls.GridMap.PlaceInteractItem;
            pointerPressed.Enable();
            pointerPressed.performed += OnPointerClicked;
        }

        private void OnDisable()
        {
            pointerHover.Disable();
            pointerPressed.Disable();
        }

        /// <summary>
        /// Updates OverGUI.
        /// </summary>
        private void Update()
        {
            OverGUI = EventSystem.current.IsPointerOverGameObject();
        }

        /// <summary>
        /// Finds current pointer positions and updates their variables.
        /// </summary>
        private void UpdateHoveredPos(InputAction.CallbackContext _)
        {
            HoveredWorldPos = Camera.main.ScreenToWorldPoint(pointerHover.ReadValue<Vector2>());
            HoveredGridPos = GridMap.GetGridPos(HoveredWorldPos);
            PointerMoved?.Invoke();
        }

        /// <summary>
        /// Gets the side of a tile the pointer is currently hovered over.
        /// </summary>
        /// <returns>The vector representing the hovered side.</returns>
        public Vector2Int GetHoveredSide()
        {
            Vector2 relativePressedPos = HoveredWorldPos - GridMap.GetCenterOfTile(HoveredGridPos);
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

        /// <summary>
        /// Gets the clickable object under the pointer, if any.
        /// </summary>
        /// <returns>The clickable object, or null if there is none.</returns>
        public Clickable GetClickableUnderPointer()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(HoveredWorldPos, Vector2.zero, 0);
            float closestDistance = float.MaxValue;
            Clickable closestClickable = null;

            foreach (RaycastHit2D hit in hits)
            {
                Clickable clickable;
                if (hit.collider.TryGetComponent(out clickable))
                {
                    if (Vector2.Distance(HoveredWorldPos, clickable.transform.position) < closestDistance)
                    {
                        closestClickable = clickable;
                    }
                }
            }

            return closestClickable;
        }

        // TODO add docs
        private void OnPointerClicked(InputAction.CallbackContext _)
        {
            if (!OverGUI)
            {
                PointerClicked?.Invoke(InputMode);
            }
        }
    }

    public enum InputMode
    {
        Interact,
        Edit,
        Remove,
        Target
    }
}
