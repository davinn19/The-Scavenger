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
            if (!InspectEnabled)
            {
                return null;
            }

            return map.GetObjectAtPos(InspectedPos);
        }

        // TODO add docs, move input detection to input handler
        private void OnInspect(InputAction.CallbackContext _)
        {
            if (inputHandler.OverGUI)
            {
                return;
            }

            Vector2Int pressedPos = inputHandler.HoveredGridPos;

            if (pressedPos == InspectedPos)
            {
                InspectEnabled = false;
                InspectedPos = Vector2Int.zero;
            }
            else
            {
                InspectEnabled = true;
                InspectedPos = inputHandler.HoveredGridPos;
            }
            InspectedObjectChanged?.Invoke();
        }

        /// <summary>
        /// Invokes changed event when the gridObject at the inspected position changes.
        /// </summary>
        /// <param name="changedPos">The gridPos that was just changed on the map.</param>
        private void OnViewedObjectSet(Vector2Int changedPos)
        {
            if (changedPos == InspectedPos)
            {
                InspectedObjectChanged?.Invoke();
            }
        }
    }
}
