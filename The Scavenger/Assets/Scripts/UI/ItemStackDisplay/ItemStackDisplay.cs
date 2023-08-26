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
        private TextMeshProUGUI stackAmountDisplay;

        public event Action AppearanceChanged;
        private ItemStack m_itemStack;
        public ItemStack ItemStack
        {
            get { return m_itemStack; }
            set // TODO add docs
            {
                Debug.Assert(value != null);

                if (m_itemStack != null && m_itemStack == value)
                {
                    return;
                }

                if (m_itemStack != null)
                {
                    m_itemStack.Changed -= UpdateAppearance;
                }

                m_itemStack = value;
                m_itemStack.Changed += UpdateAppearance;
                UpdateAppearance();
            }
        }

        [field: SerializeField] public bool ShowAmount { get; set; }
        public bool IsTransparent { get; set; }

        private void Awake()
        {
            stackAmountDisplay = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateAppearance();
        }

        private void OnEnable()
        {
            if (ItemStack != null)
            {
                ItemStack.Changed += UpdateAppearance;
            }
        }
        private void OnDestroy()
        {
            if (ItemStack != null)
            {
                ItemStack.Changed -= UpdateAppearance;
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
                itemImage.enabled = false;
                stackAmountDisplay.text = "";
            }
            else
            {
                itemImage.enabled = true;
                itemImage.sprite = ItemStack.Item.Icon;

                stackAmountDisplay.text = GetAmountString(ItemStack.Amount);
            }

            AppearanceChanged?.Invoke();
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
