using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Scavenger.UI
{
    public class GameUI : MonoBehaviour
    {
        public GameObject HoveredElement { get; private set; }
        [field: SerializeField] public Vector2 MousePos { get; private set; }

        private bool overUI = false;

        private Controls controls;
        private InputAction toggleUI;
        private InputAction pointerHover;

        public event Action<GameObject> HoveredElementChanged;


        private void Awake()
        {
            controls = new Controls();
        }

        private void Start()
        {
            UpdateHoveredElement();
        }

        private void OnEnable()
        {
            toggleUI = controls.GridMap.ToggleUI;
            toggleUI.Enable();
            toggleUI.performed += ToggleUI;

            pointerHover = controls.GridMap.PointerHover;
            pointerHover.Enable();
            pointerHover.performed += OnPointerMove;
        }

        private void OnDisable()
        {
            toggleUI.Disable();
        }

        private void Update()
        {
            overUI = EventSystem.current.IsPointerOverGameObject();
        }

        private void ToggleUI(InputAction.CallbackContext context)
        {
            // TODO make event for disabling sub-ui
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject childUI = transform.GetChild(i).gameObject;
                childUI.SetActive(!childUI.activeInHierarchy);
            }
        }

        private void OnPointerMove(InputAction.CallbackContext context)
        {
            if (overUI)
            {
                UpdateHoveredElement();
            }

            MousePos = pointerHover.ReadValue<Vector2>();
        }

        private void UpdateHoveredElement()
        {
            GameObject oldHoveredElement = HoveredElement;
            HoveredElement = null;

            PointerEventData pointerData = new(EventSystem.current) { position = pointerHover.ReadValue<Vector2>() };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerData, results);
            
            foreach (RaycastResult result in results)
            {
                InteractibleUI hoveredElement = result.gameObject.GetComponentInParent<InteractibleUI>();
                if (hoveredElement)
                {
                    HoveredElement = hoveredElement.gameObject;
                    break;
                }
            }

            if (HoveredElement != oldHoveredElement)
            {
                HoveredElementChanged?.Invoke(HoveredElement);
            }
        }
    }
}
