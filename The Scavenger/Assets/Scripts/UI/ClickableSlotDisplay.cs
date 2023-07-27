using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    /// <summary>
    /// Wraps a normal slot display to respond to clicks.
    /// </summary>
    public class ClickableSlotDisplay : MonoBehaviour
    {
        public SlotDisplay SlotDisplay { get; private set; }
        private SlotDisplayHandler handler;

        private void Awake()
        {
            SlotDisplay = GetComponentInChildren<SlotDisplay>();
            handler = GetComponentInParent<SlotDisplayHandler>();
        }

        public void OnPointerClick()
        {
            handler.OnSlotDisplayPressed(SlotDisplay);
        }
    }
}
