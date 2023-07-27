using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Temporary object that defines which spaces are permanently supported for a gridMap.
    /// </summary>
    public class SupportedPos : MonoBehaviour
    {
        /// <summary>
        /// Gets the grid position the object represents.
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetPos()
        {
            int x = Mathf.RoundToInt(transform.position.x - 0.5f);
            int y = Mathf.RoundToInt(transform.position.y - 0.5f);

            return new Vector2Int(x, y);
        }
    }
}
