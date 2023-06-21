using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private GridMap map;
        [SerializeField] private SpriteRenderer tileHover;
        [SerializeField] private GridObject obj;


        // Update is called once per frame
        void Update()
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            tileHover.transform.position = new Vector3((int)mousePos.x, (int)mousePos.y, 0);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(mousePos);
                map.PlaceObject(obj, mousePos);
            }
        }
    }
}
