using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(GridMap))]
    public class EnergyNetworks : MonoBehaviour
    {
        public List<HashSet<Conduit>> networks = new List<HashSet<Conduit>>();

        private GridMap map;

        private void Awake()
        {
            map = GetComponent<GridMap>();
        }

        public void UpdateNetworks(Vector2Int pos)
        {

        }

    }
}
