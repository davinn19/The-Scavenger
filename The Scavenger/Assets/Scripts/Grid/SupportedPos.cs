using UnityEngine;

namespace Scavenger
{
    public class SupportedPos : MonoBehaviour
    {
        public Vector2Int GetPos()
        {
            int x = Mathf.RoundToInt(transform.position.x - 0.5f);
            int y = Mathf.RoundToInt(transform.position.y - 0.5f);

            return new Vector2Int(x, y);
        }
    }
}
