using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [RequireComponent(typeof(EnergyBuffer))]
    public class MakeshiftEnergyCell : MonoBehaviour
    {
        private EnergyBuffer energyBuffer;

        private void Awake()
        {
            GetComponent<GridObject>().DataLoadRequested += LoadData;
            energyBuffer = GetComponent<EnergyBuffer>();
        }

        private void LoadData(Dictionary<string, string> data)
        {
            if (data.ContainsKey("Energy"))
            {
                energyBuffer.Energy = int.Parse(data["Energy"]); 
            }
            else
            {
                Random.Range(energyBuffer.Capacity / 2, energyBuffer.Capacity);
            }
        }

        private void Start()
        {
            
        }
    }
}
