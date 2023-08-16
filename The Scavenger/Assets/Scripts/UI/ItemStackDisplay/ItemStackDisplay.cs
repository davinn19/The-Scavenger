using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    /// <summary>
    /// Displays an itemStack.
    /// </summary>
    public class ItemStackDisplay : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI stackAmountDisplay;

        public event Action ItemStackChanged;
        private ItemStack m_itemStack;
        public ItemStack ItemStack
        {
            get { return m_itemStack; }
            set
            {
                if (m_itemStack != null)
                {
                    m_itemStack.Changed -= ItemStackChanged;
                }

                m_itemStack = value;
                m_itemStack.Changed += ItemStackChanged;
                ItemStackChanged?.Invoke();
            }
        }

        [field: SerializeField] public bool ShowAmount { get; set; }

        private void Awake()
        {
            UpdateAppearance();
            ItemStackChanged += UpdateAppearance;
        }

        private void OnDestroy()
        {
            if (ItemStack != null)
            {
                ItemStack.Changed -= ItemStackChanged;
            }
        }

        /// <summary>
        /// Constantly update the display.
        /// </summary>
        private void UpdateAppearance()
        {
            stackAmountDisplay.enabled = ShowAmount;

            if (ItemStack == null || !ItemStack)
            {
                itemImage.color = Color.clear;
                stackAmountDisplay.text = "";
            }
            else
            {
                itemImage.sprite = ItemStack.Item.Icon;
                itemImage.color = Color.white;

                stackAmountDisplay.text = GetAmountString(ItemStack.Amount);
            }
        }

        /// <summary>
        /// Gets string representation of an amount.
        /// </summary>
        /// <param name="amount">Amount to convert.</param>
        /// <returns>String representation of amount.</returns>
        private string GetAmountString(int amount)
        {
            if (amount == 1)
            {
                return "";
            }

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
