using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    public class ClickableSlotDisplay : MonoBehaviour
    {
        private SlotDisplay slotDisplay;
        private SlotDisplayHandler handler;

        private void Awake()
        {
            slotDisplay = GetComponentInChildren<SlotDisplay>();
            handler = GetComponentInParent<SlotDisplayHandler>();
        }

        public void OnPointerClick()
        {
            handler.OnSlotDisplayPressed(slotDisplay);
        }
    }
}
