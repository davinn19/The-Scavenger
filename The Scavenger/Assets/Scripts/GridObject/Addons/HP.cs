using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Tracks an object's HP.
    /// </summary>
    public class HP : MonoBehaviour, IHasPersistentData
    {
        [field: SerializeField, HideInInspector] public int Health { get; private set; }
        [SerializeField] private HPPreset MaxHP;

        // TODO add docs
        public int GetMaxHP()
        {
            if (MaxHP == HPPreset.Normal)
            {
                return 100;
            }
            return (int)MaxHP;
        }

        private void Awake()
        {
            Health = GetMaxHP();
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

    public enum HPPreset
    {
        Normal, // = 100
        Puny = 10,
        Weak = 50,
        Strong = 200,
        SuperStrong = 500,
        Invincible = 1000
    }

}

