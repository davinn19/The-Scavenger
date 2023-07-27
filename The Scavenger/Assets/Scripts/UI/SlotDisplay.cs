using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    /// <summary>
    /// Displays the itemStack in one slot of an item buffer.
    /// </summary>
    public class SlotDisplay : MonoBehaviour
    {
        [field: SerializeField] public ItemBuffer Buffer { get; private set; }
        [field: SerializeField] public int Slot { get; private set; }

        [field: SerializeField] private Image itemImage;
        private TextMeshProUGUI stackAmountDisplay;

        private void Awake()
        {
            stackAmountDisplay = GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// Changes which item buffer and slot to display.
        /// </summary>
        /// <param name="buffer">The item buffer to display.</param>
        /// <param name="slot">The slot to display.</param>
        public void SetWatchedBuffer(ItemBuffer buffer, int slot)
        {
            Buffer = buffer;
            Slot = slot;
        }

        /// <summary>
        /// Constantly update the display.
        /// </summary>
        private void Update()
        {
            if (Buffer == null)
            {
                return;
            }

            ItemStack itemStack = Buffer.GetItemInSlot(Slot);

            if (itemStack == null)
            {
                return;
            }

            if (itemStack)
            {
                itemImage.sprite = itemStack.Item.Icon;
                itemImage.color = Color.white;
                stackAmountDisplay.text = GetAmountString(itemStack.Amount);
            }
            else
            {
                itemImage.color = Color.clear;
                stackAmountDisplay.text = "";
            }

        }

        /// <summary>
        /// Gets the itemStack in the displayed item buffer and slot.
        /// </summary>
        /// <returns></returns>
        public ItemStack GetItemInSlot()
        {
            return Buffer.GetItemInSlot(Slot);
        }

        /// <summary>
        /// Gets string representation of an amount.
        /// </summary>
        /// <param name="amount">Amount to convert.</param>
        /// <returns>String representation of amount.</returns>
        private string GetAmountString(int amount)
        {
            if (amount < 1000)
            {
                return amount.ToString();
            }

            int mantissa = (int)Mathf.Log(amount, 1000f);
            string unit = "";

            switch (mantissa)
            {
                case 1:
                    unit = "k";
                    break;
                case 2:
                    unit = "m";
                    break;
                case 3:
                    unit = "b";
                    break;
                default:
                    Debug.LogError("Number is not compatible");
                    break;
            }

            string coefficient = amount.ToString()[..2];
            return coefficient[0] + "." + coefficient[1] + unit;
        }
    }
}
