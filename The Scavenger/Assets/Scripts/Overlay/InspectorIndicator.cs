using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Marks which grid position is currently inspected.
    /// </summary>
    public class InspectorIndicator : MonoBehaviour
    {
        [SerializeField] private GridObjectInspector inspector;

        private void Awake()
        {
            inspector.InspectedObjectChanged += UpdateAppearance;
            UpdateAppearance();
        }
        
        /// <summary>
        /// Updates the indicator's visibility and position based on the inspector's new values.
        /// </summary>
        private void UpdateAppearance()
        {
            if (!inspector.InspectEnabled || !inspector.GetInspectedObject())
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            transform.position = GridMap.GetCenterOfTile(inspector.InspectedPos);
        }
    }
}
