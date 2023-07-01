using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Controls movement of SpaceDebris.
    /// </summary>
    public class DebrisMotion : MonoBehaviour
    {
        [SerializeField] private float speed;

        [SerializeField] private Vector2 direction;

        [SerializeField] private float rotationSpeed;

        void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        public void SetMotion(float speed, Vector2 direction, float rotationSpeed)
        {
            this.speed = speed;
            this.direction = direction;
            this.rotationSpeed = rotationSpeed;
        }
    }
}
