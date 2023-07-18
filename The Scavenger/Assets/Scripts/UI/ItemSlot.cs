using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    [RequireComponent(typeof(Image))]
    public class ItemSlot : MonoBehaviour
    {
        public ItemStack itemStack { get; set; }
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }


        private void Update()
        {
            if (itemStack)
            {
                image.sprite = itemStack.Item.Icon; 
            }
            
        }
    }
}
