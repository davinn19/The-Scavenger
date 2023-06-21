using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class SpaceDebrisSpawner : MonoBehaviour
    {
        private const float maxDirectionTurbulence = 45;
        private const float maxRotationTurbulence = 100;

        [SerializeField]
        private List<SpaceDebris> spaceDebrisPrefabs;

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



        // Update is called once per frame
        void Update()
        {
            spawnCooldown -= Time.deltaTime;

            if (spawnCooldown <= 0)
            {
                Spawn();
                spawnCooldown = 1 / spawnRate;
            }

        }

        private void Spawn()
        {
            SpaceDebris newDebris = Instantiate(spaceDebrisPrefabs[Random.Range(0, spaceDebrisPrefabs.Count)]);

            float speed = Random.Range(debrisSpeed, turbulence * debrisSpeed);

            float rotationTurbulence = turbulence * maxRotationTurbulence;
            float rotationSpeed = Random.Range(-rotationTurbulence, rotationTurbulence);

            float directionTurbulence = turbulence * maxDirectionTurbulence;
            float angle = (direction + Random.Range(-directionTurbulence, directionTurbulence)) * Mathf.Deg2Rad;

            Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));


            newDebris.GetComponent<DebrisMotion>().SetMotion(speed, directionVector, rotationSpeed);

        }


    }
}