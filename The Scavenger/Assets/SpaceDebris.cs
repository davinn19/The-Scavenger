using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class SpaceDebris : MonoBehaviour
    {
        private void Update()
        {
            
        }

        private void OnMouseDown()
        {
            Debug.Log("Debris collected");
            Destroy(gameObject);
        }
    }
}
