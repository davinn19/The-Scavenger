using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Dictionary<Item, int> inventory = new Dictionary<Item, int>();


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}