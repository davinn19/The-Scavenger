using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Entity representing an item floating in space. Can be clicked to give the item.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Clickable))]
    public class FloatingItem : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Clickable clickable;

        [SerializeField] private ItemStack itemStack;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            clickable = GetComponent<Clickable>();
            clickable.Clicked += TryGiveItems;
        }

        /// <summary>
        /// Sets a floating item's contents.
        /// </summary>
        /// <param name="itemStack"></param>
        public void Init(ItemStack itemStack)
        {
            Debug.Assert(itemStack);
            Debug.Assert(!itemStack.IsEmpty());

            this.itemStack = itemStack;
            spriteRenderer.sprite = itemStack.Item.Icon;
        }

        /// <summary>
        /// Attempts to give the floating item's contents to the player's inventory.
        /// </summary>
        /// <param name="gameManager">The current gameManager.</param>
        private void TryGiveItems(GameManager gameManager)
        {
            // Fails if it cannot give all of its contents at once. TODO add interaction with held item handler
            int remainder = gameManager.Inventory.Insert(itemStack, itemStack.Amount, true);
            if (remainder == 0)
            {
                gameManager.Inventory.Insert(itemStack);
                Destroy(gameObject);
            }
        }

    }
}
