using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
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

            slotDisplay.Buffer = silo.ItemBuffer;
            slotDisplay.Slot = 0;

            UpdateLockButtonStatus();
        }

        public void OnLockButtonPressed()
        {
            silo.ItemBuffer.ToggleLocked(0);
            UpdateLockButtonStatus();
        }

        private void UpdateLockButtonStatus()
        {
            bool locked = silo.ItemBuffer.IsLocked(0);
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
