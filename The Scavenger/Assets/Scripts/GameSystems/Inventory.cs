using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class Inventory : MonoBehaviour
    {
        public const int InventoryWidth = 7;
        public const int InventoryHeight = 4;


        private ItemStack[,] inventory = new ItemStack[InventoryWidth, InventoryHeight];

        private void Awake()
        {
            foreach (ItemStack startingItemStack in GetComponentsInChildren<ItemStack>())
            {
                Vector2Int pos = GetFirstEmptyPos();
                SetItemStack(startingItemStack, pos);
            }
        }

        public ItemStack GetItemStack(int x, int y)
        {
            return inventory[x, y];
        }

        public ItemStack GetItemStack(Vector2Int pos) => GetItemStack(pos.x, pos.y);

        private void SetItemStack(ItemStack itemStack, Vector2Int pos)
        {
            inventory[pos.x, pos.y] = itemStack;
        }



        private Vector2Int GetFirstEmptyPos()
        {
            for (int y = 0; y < InventoryHeight; y++)
            {
                for (int x = 0; x < InventoryWidth; x++)
                {
                    if (GetItemStack(x, y) == null)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            return new Vector2Int(-1, -1);
        }
    }
}
