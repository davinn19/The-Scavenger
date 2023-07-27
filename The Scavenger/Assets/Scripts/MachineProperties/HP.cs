using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Tracks an object's HP.
    /// </summary>
    public class HP : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        public int Health { get; private set; }

        public void Awake()
        {
            Health = MaxHealth;
        }

        /// <summary>
        /// Sets the max health if it hasn't been set in inspector already.
        /// </summary>
        /// <param name="maxHealth"></param>
        public void SetMaxHealth(int maxHealth)
        {
            if (MaxHealth <= 0)
            {
                MaxHealth = maxHealth;
                Health = maxHealth;
            }
        }
    }

}

