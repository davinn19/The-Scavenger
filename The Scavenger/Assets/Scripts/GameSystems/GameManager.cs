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


        void Update()
        {
            Item item = GetComponent<ItemSelection>().GetSelectedItem();
            Sprite placementPreview = item.Icon;
            tileHover.sprite = placementPreview;

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2Int gridPos = new Vector2Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));

            tileHover.transform.position = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(mousePos);
                map.TryInteract(item, gridPos);
            }
        }
    }
}
