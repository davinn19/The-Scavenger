using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Tracks an object's HP.
    /// </summary>
    public class HP : MonoBehaviour, IHasPersistentData
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Health { get; private set; }

        public void Init(int maxHealth)
        {
            // TODO implement
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

        // TODO add docs
        void IHasPersistentData.ReadPersistentData(PersistentData data)
        {
            if (data.ContainsKey("HP"))
            {
                Health = data.GetInt("HP");
            }
        }

        // TODO add docs
        PersistentData IHasPersistentData.WritePersistentData(PersistentData data)
        {
            data.Add("HP", Health);
            return data;
        }
    }

}

