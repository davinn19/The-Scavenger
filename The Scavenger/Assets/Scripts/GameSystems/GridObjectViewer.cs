using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger
{
    public class GridObjectViewer : MonoBehaviour
    {
        private Vector2Int viewingPos;
        [SerializeField] private SpriteRenderer viewIndicator;

        private Controls controls;
        private InputAction view;

        private GridHover gridHover;
        private GameManager gameManager;

        private bool viewEnabled = false;

        public event Action ViewedObjectChanged;

        private void Awake()
        {
            controls = new Controls();

            gridHover = GetComponent<GridHover>();
            gameManager = GetComponent<GameManager>();
            gameManager.Map.GridObjectSet += OnViewedObjectSet;
        }

        private void OnEnable()
        {
            view = controls.GridMap.ViewGridObject;
            view.Enable();
            view.performed += OnView;
        }

        private void OnDisable()
        {
            view.Disable();
        }

        public GridObject GetViewedObject()
        {
            if (!viewEnabled)
            {
                return null;
            }

            return gameManager.Map.GetObjectAtPos(viewingPos);
        }


        private void OnView(InputAction.CallbackContext context) // Right click to view gridObject
        {
            if (gridHover.OverGUI)
            {
                viewEnabled = false;
                viewIndicator.enabled = false;
                viewingPos = Vector2Int.zero;
                return;
            }
            else
            {
                viewEnabled = true;
                viewingPos = gridHover.HoveredPos;

                viewIndicator.enabled = true;
                viewIndicator.transform.position = GridMap.GetCenterOfTile(viewingPos);
            }

            ViewedObjectChanged.Invoke();
        }

        private void OnViewedObjectSet(Vector2Int changedPos)
        {
            if (changedPos == viewingPos)
            {
                ViewedObjectChanged.Invoke();
            }
        }
    }
}
