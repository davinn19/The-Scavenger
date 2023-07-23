using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(Clickable))]
    public class SpaceDebris : MonoBehaviour
    {
        [SerializeField] private Item spaceDebrisItem;

        private void Awake()
        {
            GetComponent<Clickable>().Clicked += OnClick;
        }

        private void OnClick(GameManager gameManager)
        {
            ItemStack itemDrop = new ItemStack(spaceDebrisItem, 1);
            int remainder = gameManager.Inventory.Insert(itemDrop);
            if (remainder == 0)
            {
                Destroy(gameObject);
            }

        }
    }
}
