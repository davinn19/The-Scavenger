using Scavenger.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger
{
    [RequireComponent(typeof(ItemBuffer))]
    public class ItemSelection : MonoBehaviour
    {
        private int m_SelectedSlot = 0;
        public int SelectedSlot
        {
            get
            {
                return m_SelectedSlot;
            }
            set
            {
                Debug.Assert(value < inventory.NumSlots);
                m_SelectedSlot = value;
            }
        }

        private ItemBuffer inventory;


        private void Awake()
        {
            inventory = GetComponent<ItemBuffer>();
        }

        public ItemStack GetSelectedItemStack()
        {
            return inventory.GetItemInSlot(SelectedSlot);
        }

    }
}
