using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Gives objects floating motion.
    /// </summary>
    public class FloatingMotion : MonoBehaviour
    {
        public float Speed { get; set; }
        public Vector2 Direction { get; set; }
        public float RotationSpeed { get; set; }

        void Update()
        {
            transform.Translate(Speed * Time.deltaTime * Direction, Space.World);
            transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Sets the debris' motion variables.
        /// </summary>
        /// <param name="speed">The debris' new speed.</param>
        /// <param name="direction">The debris' new direction.</param>
        /// <param name="rotationSpeed">The debris' new rotation speed.</param>
        public void SetMotion(float speed, Vector2 direction, float rotationSpeed)
        {
            Speed = speed;
            Direction = direction;
            RotationSpeed = rotationSpeed;
        }


    }
}
