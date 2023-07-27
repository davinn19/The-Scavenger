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

        private InputHandler inputHandler;
        private GridMap map;

        public event Action InspectedObjectChanged;

        private void Awake()
        {
            GameManager gameManager = GetComponent<GameManager>();

            inputHandler = gameManager.InputHandler;
            inputHandler.PointerClicked += OnPointerClick;

            map = gameManager.Map;
            map.GridObjectSet += OnViewedObjectSet;
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

        // TODO add docs, rethink inputs
        private void OnPointerClick(InputMode inputMode)
        {
            if (inputMode != InputMode.Inspect)
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
