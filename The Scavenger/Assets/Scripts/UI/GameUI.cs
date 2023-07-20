using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Scavenger.UI
{
    public class GameUI : MonoBehaviour
    {
        private Controls controls;
        private InputAction toggleUI;

        private void Awake()
        {
            controls = new Controls();
        }

        private void OnEnable()
        {
            toggleUI = controls.GridMap.ToggleUI;
            toggleUI.Enable();
            toggleUI.performed += ToggleUI;
        }

        private void OnDisable()
        {
            toggleUI.Disable();
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

        


    }
}
