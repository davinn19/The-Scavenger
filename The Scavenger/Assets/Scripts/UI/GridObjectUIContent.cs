using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    public class GridObjectUIContent : MonoBehaviour
    {
        protected GridObject gridObject { get; set; }

        private void Awake()
        {
            enabled = false;
        }

        public virtual void Init(GridObject gridObject)
        {
            this.gridObject = gridObject;
            enabled = true;
        }
    }
}
