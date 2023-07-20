using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private ItemSelection itemSelection;
        [SerializeField] private Image itemImage;

        private TextMeshProUGUI itemName;

        private void Awake()
        {
            itemName = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            // TODO link to event
            ItemStack itemStack = itemSelection.GetSelectedItemStack();
            if (!itemStack)
            {
                itemImage.color = Color.clear;
                itemName.text = "";
            }
            else
            {
                itemImage.sprite = itemStack.Item.Icon;
                itemImage.color = Color.white;
                itemName.text = itemStack.Item.DisplayName;
            }
        }
    }
}
