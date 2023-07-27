using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public override void Init(GridObject gridObject)
        {
            base.Init(gridObject);

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
            silo.Buffer.ToggleLocked(0);
            UpdateLockButtonStatus();
        }

        /// <summary>
        /// Changes the locked button's appearance to match the silo's locked status.
        /// </summary>
        private void UpdateLockButtonStatus()
        {
            bool locked = silo.Buffer.IsLocked(0);
            if (locked)
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
