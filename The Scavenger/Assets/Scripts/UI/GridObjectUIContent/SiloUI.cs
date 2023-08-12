using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scavenger.GridObjectBehaviors;

namespace Scavenger.UI.UIContent
{
    /// <summary>
    /// Controls the inspector UI for silo objects.
    /// </summary>
    public class SiloUI : GridObjectUIContent
    {
        private Silo silo;

        [SerializeField] private SlotDisplay slotDisplay;
        [SerializeField] private Button lockButton;
        [SerializeField] private TextMeshProUGUI lockButtonText;

        public override void Init(GridObject gridObject, PlayerInventory inventory)
        {
            base.Init(gridObject, inventory);

            silo = gridObject.GetComponent<Silo>();
            silo.Buffer.SlotChanged += (_) => UpdateLockButtonStatus();
            slotDisplay.SetWatchedBuffer(silo.Buffer, 0);

            UpdateLockButtonStatus();
        }

        /// <summary>
        /// Toggles the silo's locked status when the button is pressed.
        /// </summary>
        public void OnLockButtonPressed()
        {
            silo.Buffer.ToggleLocked();
            UpdateLockButtonStatus();
        }

        /// <summary>
        /// Changes the locked button's appearance to match the silo's locked status.
        /// </summary>
        private void UpdateLockButtonStatus()
        {
            if (silo.IsLocked())
            {
                lockButtonText.text = "Locked";
            }
            else
            {
                lockButtonText.text = "Unlocked";
            }
        }
    }
}
