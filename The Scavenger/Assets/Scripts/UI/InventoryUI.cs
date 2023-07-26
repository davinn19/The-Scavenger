using UnityEngine;

namespace Scavenger.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUI : MonoBehaviour
    {
        public const int InventoryWidth = 7;
        public const int InventoryHeight = 4;

        [SerializeField] private ItemSelection itemSelection;
        [SerializeField] private ItemBuffer inventory;
        private SlotDisplay[] slotDisplays;

        private void Awake()
        {
            InitSlotDisplays();
        }

        private void InitSlotDisplays()
        {
            slotDisplays = GetComponentsInChildren<SlotDisplay>();

            for (int slot = 0; slot < slotDisplays.Length; slot++)
            {
                SlotDisplay slotDisplay = slotDisplays[slot];
                slotDisplay.SetWatchedBuffer(inventory, slot);
            }
        }
    }
}
