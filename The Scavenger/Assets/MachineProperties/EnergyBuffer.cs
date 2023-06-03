using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class EnergyBuffer : MonoBehaviour
    {
        [SerializeField] private int maxEnergy;
        [SerializeField] private int energy = 0;
        [SerializeField] private int maxOutput;
        [SerializeField] private int maxInput;
    }

}
