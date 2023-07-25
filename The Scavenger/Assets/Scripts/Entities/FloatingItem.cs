using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Clickable))]
    public class FloatingItem : MonoBehaviour
    {
        [field: SerializeField] public ItemStack ItemStack { get; private set; }
        private SpriteRenderer spriteRenderer;
        private Clickable clickable;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            clickable = GetComponent<Clickable>();
            clickable.Clicked += OnClick;
        }

        public void Init(ItemStack itemStack)
        {
            Debug.Assert(itemStack);
            Debug.Assert(!itemStack.IsEmpty());

            ItemStack = itemStack;
            spriteRenderer.sprite = itemStack.Item.Icon;
        }

        private void OnClick(GameManager gameManager)
        {
            int remainder = gameManager.Inventory.Insert(ItemStack, ItemStack.Amount, true);
            if (remainder == 0)
            {
                gameManager.Inventory.Insert(ItemStack);
                Destroy(gameObject);
            }
        }

    }
}
