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
            ItemStack itemStack = GetComponent<ItemSelection>().GetSelectedItemStack();
            Sprite placementPreview = itemStack.item.GetIcon();
            tileHover.sprite = placementPreview;

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2Int gridPos = new Vector2Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));

            tileHover.transform.position = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0);

            GridObject gridObject = map.GetObjectAtPos(gridPos);

            tileHover.enabled = gridObject == null;

            if (Input.GetMouseButtonDown(0))    // Left click to place/interact
            {
                // TODO uncomment

                //// Place Item
                //if (!gridObject && itemStack.item is PlaceableItem)         // Clicked on empty space with placable object, place the object
                //{
                //    map.TryPlaceItem(itemStack.item as PlaceableItem, gridPos);
                //}
                //else if (gridObject)    // Interact item
                //{
                //    Vector2Int sidePressed = GetSidePressed(mousePos, tileHover.transform.position);
                //    map.TryInteract(itemStack, gridPos, sidePressed);
                //}
                
            }
            else if (Input.GetMouseButtonDown(1))   // Right click to open menu/view info
            {
                // TODO implement
            }
        }

        private Vector2Int GetSidePressed(Vector3 mousePos, Vector3 tilePos)
        {
            Vector3 relativePressedPos = mousePos - tilePos;
            Vector2Int greaterAxisPressed;
            int directionPressed;

            if (Mathf.Abs(relativePressedPos.x) > Mathf.Abs(relativePressedPos.y))
            {
                greaterAxisPressed = Vector2Int.right;
                directionPressed = Mathf.CeilToInt(Mathf.Sign(relativePressedPos.x));
            }
            else
            {
                greaterAxisPressed = Vector2Int.up;
                directionPressed = Mathf.CeilToInt(Mathf.Sign(relativePressedPos.y));
            }

            return greaterAxisPressed * directionPressed;
        }
    }
}
