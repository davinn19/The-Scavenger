using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Periodically spawns space debris.
    /// </summary>
    public class SpaceDebrisSpawner : MonoBehaviour
    {
        private const float maxDirectionTurbulence = 45;
        private const float maxRotationTurbulence = 100;

        [SerializeField]
        private List<GameObject> spaceDebrisPrefabs;

        [SerializeField]
        private float debrisSpeed;

        [SerializeField]
        [Range(0, 360)]
        private float direction;

        [SerializeField]
        [Range(0, 1)]
        private float turbulence;

        [SerializeField]
        [Tooltip("Number of spawns per second")]
        private float spawnRate;    // spawns per second

        [SerializeField]
        private Vector2 simulationRange;

        private float spawnCooldown = 0;

        /// <summary>
        /// Runs the cooldown timer and spawns a space debris at the end.
        /// </summary>
        private void Update()
        {
            spawnCooldown -= Time.deltaTime;

            if (spawnCooldown <= 0)
            {
                Spawn();
                spawnCooldown = 1 / spawnRate;
            }
        }

        /// <summary>
        /// Spawns a new space debris based on the spawner's parameters.
        /// </summary>
        private void Spawn()
        {
            GameObject newDebris = Instantiate(spaceDebrisPrefabs[Random.Range(0, spaceDebrisPrefabs.Count)]);
            newDebris.transform.position = transform.position;

            float speed = Random.Range(debrisSpeed, turbulence * debrisSpeed);

            float rotationTurbulence = turbulence * maxRotationTurbulence;
            float rotationSpeed = Random.Range(-rotationTurbulence, rotationTurbulence);

            float directionTurbulence = turbulence * maxDirectionTurbulence;
            float angle = (direction + Random.Range(-directionTurbulence, directionTurbulence)) * Mathf.Deg2Rad;

            Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            newDebris.GetComponent<FloatingMotion>().SetMotion(speed, directionVector, rotationSpeed);
        }


    }
}
