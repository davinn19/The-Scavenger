using UnityEngine;

namespace Scavenger.UI
{
    /// <summary>
    /// Controls UI for the player's inventory.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUI : MonoBehaviour
    {
        public const int InventoryWidth = 7;
        public const int InventoryHeight = 4;

        [SerializeField] private GameManager gameManager;

        private void Start()
        {
            InitSlotDisplays();
        }

        /// <summary>
        /// Assigns each slot display to the proper slot in the player's inventory.
        /// </summary>
        private void InitSlotDisplays()
        {
            SlotDisplay[] slotDisplays = GetComponentsInChildren<SlotDisplay>();

            for (int slot = 0; slot < slotDisplays.Length; slot++)
            {
                SlotDisplay slotDisplay = slotDisplays[slot];
                slotDisplay.SetWatchedBuffer(gameManager.Inventory, slot);
            }
        }
    }
}
