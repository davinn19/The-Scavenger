using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(Inventory))]
    public class ItemSelection : MonoBehaviour
    {
        [SerializeField] private int selectedItem = 0;

        private Inventory inventory;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
        }

        private void Update()
        {
            float scrollDirection = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scrollDirection < 0)
            {
                selectedItem++;
                if (selectedItem >= Inventory.InventoryWidth)
                {
                    selectedItem = 0;
                }
            }
            else if (scrollDirection > 0)
            {
                selectedItem--;
                if (selectedItem < 0)
                {
                    selectedItem = Inventory.InventoryWidth - 1;
                }
            }
        }

        public ItemStack GetSelectedItemStack()
        {
            return inventory.GetItemStack(selectedItem, 0);
        }

    }
}
