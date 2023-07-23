using UnityEngine;

namespace Scavenger
{
    public class HP : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        public int Health { get; private set; }

        public void Awake()
        {
            Health = MaxHealth;
        }

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

