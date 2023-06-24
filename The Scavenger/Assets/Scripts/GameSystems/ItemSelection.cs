using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemSelection : MonoBehaviour
    {
        [SerializeField] private List<Item> items = new List<Item>();
        [SerializeField] private int selectedItem = 0;

        private void Update()
        {
            float scrollDirection = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scrollDirection > 0)
            {
                selectedItem++;
                if (selectedItem >= items.Count)
                {
                    selectedItem = 0;
                }
            }
            else if (scrollDirection < 0)
            {
                selectedItem--;
                if (selectedItem < 0)
                {
                    selectedItem = items.Count - 1;
                }
            }
        }

        public Item GetSelectedItem()
        {
            return items[selectedItem];
        }

    }
}
