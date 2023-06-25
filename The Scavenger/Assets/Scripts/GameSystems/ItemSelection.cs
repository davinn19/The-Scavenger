using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemSelection : MonoBehaviour
    {
        [SerializeField] private int selectedItem = 0;

        private void Update()
        {
            ItemStack[] inventory = GetComponentsInChildren<ItemStack>();
            float scrollDirection = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scrollDirection > 0)
            {
                selectedItem++;
                if (selectedItem >= inventory.Length)
                {
                    selectedItem = 0;
                }
            }
            else if (scrollDirection < 0)
            {
                selectedItem--;
                if (selectedItem < 0)
                {
                    selectedItem = inventory.Length - 1;
                }
            }
        }

        public ItemStack GetSelectedItemStack()
        {
            return GetComponentsInChildren<ItemStack>()[selectedItem];
        }

    }
}
