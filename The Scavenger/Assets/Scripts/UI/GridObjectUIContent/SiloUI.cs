using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scavenger.UI
{
    public class SiloUI : GridObjectUIContent
    {
        public Silo Silo { get; set; }

        private SlotDisplay slotDisplay;
        private Button lockButton;
        private TextMeshProUGUI lockButtonText;


        private void Awake()
        {
            slotDisplay = GetComponentInChildren<SlotDisplay>();
            slotDisplay.Buffer = Silo.ItemBuffer;
            slotDisplay.Slot = 0;

            lockButton = GetComponentInChildren<Button>();
            lockButtonText = lockButton.GetComponent<TextMeshProUGUI>();

            UpdateLockButtonStatus();
        }

        private void UpdateLockButtonStatus()
        {
            bool locked = Silo.ItemBuffer.IsLocked(0);
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
