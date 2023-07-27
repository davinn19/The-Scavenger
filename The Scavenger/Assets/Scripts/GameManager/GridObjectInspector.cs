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
        private Vector2Int inspectedPos;

        private Controls controls;
        private InputAction inspect;

        private InputHandler inputHandler;
        private GridMap map;
        private GameManager gameManager;

        private bool inspectEnabled = false;

        public event Action InspectedObjectChanged;

        private void Awake()
        {
            GameManager gameManager = GetComponent<GameManager>();

            inputHandler = gameManager.InputHandler;
            map = gameManager.Map;
            map.GridObjectSet += OnViewedObjectSet;

            controls = new Controls();
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
            if (!inspectEnabled)
            {
                return null;
            }

            return gameManager.Map.GetObjectAtPos(inspectedPos);
        }

        // TODO implement inspector indicator elsewhere

        /// <summary>
        /// Invokes changed event when the inspected position is changed.
        /// </summary>
        private void OnInspect(InputAction.CallbackContext _)
        {
            if (inputHandler.OverGUI)
            {
                inspectEnabled = false;
                inspectedPos = Vector2Int.zero;
                return;
            }
            else
            {
                inspectEnabled = true;
                inspectedPos = inputHandler.HoveredGridPos;
            }

            InspectedObjectChanged?.Invoke();
        }

        /// <summary>
        /// Invokes changed event when the gridObject at the inspected position changes.
        /// </summary>
        /// <param name="changedPos">The gridPos that was just changed on the map.</param>
        private void OnViewedObjectSet(Vector2Int changedPos)
        {
            if (changedPos == inspectedPos)
            {
                InspectedObjectChanged?.Invoke();
            }
        }
    }
}
