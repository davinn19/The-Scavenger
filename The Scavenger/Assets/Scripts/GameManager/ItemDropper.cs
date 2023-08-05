using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Generates floating items from item stacks.
    /// </summary>
    public class ItemDropper : MonoBehaviour
    {
        [SerializeField] private FloatingItem floatingItemPrefab;

        /// <summary>
        /// Creates a new floating item.
        /// </summary>
        /// <param name="itemStack">The itemStack stored in the floating item.</param>
        /// <param name="worldPos">The position to initialize the floating item at.</param>
        /// <returns>The new floating item.</returns>
        public FloatingItem CreateFloatingItem(ItemStack itemStack, Vector2 worldPos)
        {
            FloatingItem floatingItem = Instantiate(floatingItemPrefab);
            floatingItem.transform.position = worldPos;
            floatingItem.Init(itemStack);

            (float, Vector2, float) motionStats = GenerateFloatingMotionStats();
            floatingItem.GetComponent<FloatingMotion>().SetMotion(motionStats.Item1, motionStats.Item2, motionStats.Item3);

            return floatingItem;
        }

        /// <summary>
        /// Generates random values to define a floating item's motion.
        /// </summary>
        /// <returns>3-tuple with the generated speed, direction, and rotation speed, in that order.</returns>
        private (float, Vector2, float) GenerateFloatingMotionStats()
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            return (Random.Range(0.2f, 0.5f), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), Random.Range(10f, 25f));
        }
    }
}
