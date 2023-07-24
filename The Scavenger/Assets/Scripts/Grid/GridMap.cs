using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Stores information on gridObjects in the world.
    /// </summary>
    public class GridMap : MonoBehaviour
    {
        public static readonly Vector2Int[] adjacentDirections = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        public UpdatePropagation updatePropagation; // TODO combine the two updates???
        public UpdateCycle updateCycle;

        public event Action<Vector2Int> GridObjectSet;

        private readonly Dictionary<Vector2Int, GridChunk> gridChunks = new();

        private readonly HashSet<Vector2Int> supportedPos = new();


        private void Awake()
        {
            InitSupportedPos();
        }

        /// <summary>
        /// Sets position of each gridObject placed in the editor as a supported pos, then removes it.
        /// </summary>
        private void InitSupportedPos()
        {
            foreach (SupportedPos pos in GetComponentsInChildren<SupportedPos>())
            {
                supportedPos.Add(pos.GetPos());
                Destroy(pos.gameObject);
            }
        }

        /// <summary>
        /// Checks if a position is permanently supported.
        /// </summary>
        /// <param name="gridPos">Position to check.</param>
        /// <returns></returns>
        public bool IsSupported(Vector2Int gridPos)
        {
            return supportedPos.Contains(gridPos);
        }

        /// <summary>
        /// Gets the chunk at the chunk index, creates it if nonexistent.
        /// </summary>
        /// <param name="chunkIndex">Index to search with.</param>
        /// <returns>GridChunk with the chunkIndex.</returns>
        public GridChunk GetChunk(Vector2Int chunkIndex)
        {
            if (!gridChunks.ContainsKey(chunkIndex))
            {
                return CreateChunk(chunkIndex);
            }

            return gridChunks[chunkIndex];
        }

        /// <summary>
        /// Creates a gridChunk at the chunk index.
        /// </summary>
        /// <param name="chunkIndex">Index to create the gridChunk with.</param>
        /// <returns>Created gridChunk.</returns>
        private GridChunk CreateChunk(Vector2Int chunkIndex)
        {
            GameObject chunkObject = new(chunkIndex.ToString());
            chunkObject.transform.parent = transform;
            chunkObject.transform.localPosition = new Vector3(chunkIndex.x, chunkIndex.y, 0) * GridChunk.ChunkSize;

            GridChunk gridChunk = chunkObject.AddComponent<GridChunk>();
            gridChunk.ChunkIndex = chunkIndex;

            gridChunks.Add(chunkIndex, gridChunk);

            return gridChunk;
        }

        /// <summary>
        /// Gets the gridChunk that occupies a position.
        /// </summary>
        /// <param name="gridPos">Position to search with.</param>
        /// <returns>GridChunk that occupies gridPos.</returns>
        public GridChunk GetChunkAtPos(Vector2Int gridPos)
        {
            Vector2Int chunkIndex = GridChunk.GetChunkIndex(gridPos);
            return GetChunk(chunkIndex);
        }

        /// <summary>
        /// Sets a grid object at a position.
        /// </summary>
        /// <param name="gridObject">Prefab of the new gridObject.</param>
        /// <param name="gridPos">Position to set the new gridObject at.</param>
        private void SetObjectAtPos(GridObject gridObject, Vector2Int gridPos)
        {
            GridChunk chunk = GetChunkAtPos(gridPos);
            chunk.SetObjectAtPos(gridObject, gridPos);
            GridObjectSet.Invoke(gridPos);
        }

        /// <summary>
        /// Gets the gridObject at a position.
        /// </summary>
        /// <param name="gridPos">Position to search with.</param>
        /// <returns>GridObject at the specified position.</returns>
        public GridObject GetObjectAtPos(Vector2Int gridPos)
        {
            GridChunk chunk = GetChunkAtPos(gridPos);
            return chunk.GetObject(gridPos);
        }

        /// <summary>
        /// Gets the object at the relative grid position.
        /// </summary>
        /// <param name="origin">Base grid position.</param>
        /// <param name="relativePos">Offset from the base position.</param>
        /// <returns></returns>
        public GridObject GetObjectAtPos(Vector2Int origin, Vector2Int relativePos) => GetObjectAtPos(relativePos + origin);

        /// <summary>
        /// Attempts to interact with the gridObject at a position using an item.
        /// </summary>
        /// <param name="inventory">The player's inventory.</param>
        /// <param name="slot">The player's equipped slot.</param>
        /// <param name="gridPos">Position of gridObject to interact with.</param>
        /// <param name="sidePressed">Side of gridObject that was pressed.</param>
        /// <returns>True if an interaction happened.</returns>
        public bool TryObjectInteract(ItemBuffer inventory, int slot, Vector2Int gridPos, Vector2Int sidePressed)
        {
            GridObject existingObject = GetObjectAtPos(gridPos);
            if (existingObject && existingObject.TryInteract != null)
            {
                return existingObject.TryInteract(inventory, slot, sidePressed);
            }

            return false;
        }

        /// <summary>
        /// Attempts to place the item at a position.
        /// </summary>
        /// <param name="inventory">The player's inventory.</param>
        /// <param name="slot">The player's equipped slot.</param>
        /// <param name="gridPos">Position to place the item at.</param>
        /// <returns>True if placement was successful.</returns>
        public bool TryPlaceItem(ItemBuffer inventory, int slot, Vector2Int gridPos)// TODO add supported constraint
        {
            // Space must be empty to place new object
            GridObject existingObject = GetObjectAtPos(gridPos);
            if (existingObject)
            {
                return false;
            }

            // Must be holding an item
            ItemStack itemStack = inventory.GetItemInSlot(slot);
            if (!itemStack || itemStack.IsEmpty())
            {
                return false;
            }

            // Item must have PlacedObject property
            if (!itemStack.Item.TryGetProperty(out PlacedObject placedObject))
            {
                return false;
            }

            SetObjectAtPos(placedObject.Object, gridPos);
            updatePropagation.HandlePlaceUpdate(gridPos);
            inventory.Extract(slot, 1);
            return true;
        }

        /// <summary>
        /// Gets center of grid position, used for placing sprites.
        /// </summary>
        /// <param name="gridPos">Position to calculate with.</param>
        /// <returns>Center of grid position.</returns>
        public static Vector2 GetCenterOfTile(Vector2Int gridPos)
        {
            return gridPos + Vector2.one * 0.5f;
        }

    }
}
