using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemSelection : MonoBehaviour
    {
        [SerializeField] private Item[] items;
        [SerializeField] private int selectedItem = 0;

        private void Update()
        {
            float scrollDirection = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scrollDirection > 0)
            {
                selectedItem++;
                if (selectedItem >= items.Length)
                {
                    selectedItem = 0;
                }
            }
            else if (scrollDirection < 0)
            {
                selectedItem--;
                if (selectedItem < 0)
                {
                    selectedItem = items.Length - 1;
                }
            }
        }

        public Item GetSelectedItem()
        {
            return items[selectedItem];
        }

    }
}
