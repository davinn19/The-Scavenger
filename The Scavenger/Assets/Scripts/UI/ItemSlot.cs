using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    [RequireComponent(typeof(Image))]
    public class ItemSlot : MonoBehaviour
    {
        public ItemStack itemStack { get; set; }
        private Image image;

        private TextMeshProUGUI stackAmountDisplay;
        private void Awake()
        {
            image = GetComponent<Image>();
            stackAmountDisplay = GetComponentInChildren<TextMeshProUGUI>();
        }


        private void Update()
        {
            if (itemStack == null)
            {
                return;
            }

            if (itemStack)
            {
                image.sprite = itemStack.Item.Icon;
                stackAmountDisplay.text = GetAmountString(itemStack.amount);
            }
            else
            {
                image.sprite = null;
                stackAmountDisplay.text = "";
            }
            
        }

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
