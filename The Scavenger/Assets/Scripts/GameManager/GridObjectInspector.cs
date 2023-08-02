using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger
{
    /// <summary>
    /// Tracks the grid object that occupies a specific position.
    /// </summary>
    public class GridObjectInspector : MonoBehaviour
    {
        public Vector2Int InspectedPos { get; private set; }
        public bool InspectEnabled { get; private set; }

        private GridMap map;

        private InputHandler inputHandler;
        private Controls controls;
        private InputAction inspect;

        public event Action InspectedObjectChanged;

        private void Awake()
        {
            GameManager gameManager = GetComponent<GameManager>();

            inputHandler = gameManager.InputHandler;
            controls = new Controls();

            map = gameManager.Map;
            map.GridObjectSet += OnViewedObjectSet;
        }

        private void OnEnable()
        {
            inspect = controls.GridMap.ViewGridObject;
            inspect.Enable();
            inspect.performed += OnInspect;
        }

        private void OnDisable()
        {
            inspect.Disable();
        }

        /// <summary>
        /// Gets the gridObject at the current inspected gridPos.
        /// </summary>
        /// <returns>The gridObject at the inspected gridPos.</returns>
        public GridObject GetInspectedObject()
        {
            return map.GetObjectAtPos(InspectedPos);
        }

        // TODO add docs, move input detection to input handler
        private void OnInspect(InputAction.CallbackContext _)
        {
            if (inputHandler.OverGUI)
            {
                return;
            }

            // Prevent multiple UI redraws when clicking on the same position
            Vector2Int pressedPos = inputHandler.HoveredGridPos;
            if (InspectEnabled && InspectedPos == pressedPos)
            {
                return;
            }

            InspectedPos = pressedPos;
            InspectEnabled = GetInspectedObject();  // Disables inspect mode if inspecting on an empty space

            InspectedObjectChanged?.Invoke();
        }

        /// <summary>
        /// Invokes changed event when the gridObject at the inspected position changes.
        /// </summary>
        /// <param name="changedPos">The gridPos that was just changed on the map.</param>
        private void OnViewedObjectSet(Vector2Int changedPos)
        {
            // Ignore if the changed pos is not the one being inspected
            if (changedPos != InspectedPos)
            {
                return;
            }

            // Ignore if inspector is already disabled, cannot be reactivated
            if (!InspectEnabled)
            {
                return;
            }

            // Disables inspect mode if now inspecting on an empty space
            InspectEnabled = GetInspectedObject();  

            InspectedObjectChanged?.Invoke();
        }
    }
}
