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
        [SerializeField] private Image itemImage;

        private TextMeshProUGUI stackAmountDisplay;
        private void Awake()
        {
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
                itemImage.sprite = itemStack.Item.Icon;
                itemImage.color = Color.white;
                stackAmountDisplay.text = GetAmountString(itemStack.amount);
            }
            else
            {
                itemImage.color = Color.clear;
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
