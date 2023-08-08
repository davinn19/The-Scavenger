using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [CreateAssetMenu(fileName = "GridObjectDatabase", menuName = "Scavenger/Database/GridObjects")]
    public class GridObjectDatabase : PrefabDatabase<GridObject>
    {
        [SerializeField] private GridObject[] gridObjects;

        public GridObject Get(string id)
        {
            GridObject cacheResult = GetFromCache(id);
            if (cacheResult)
            {
                return cacheResult;
            }

            foreach (GridObject gridObject in gridObjects)
            {
                if (gridObject.name == id)
                {
                    AddToCache(gridObject.name, gridObject);
                    return gridObject;
                }
            }
            throw new ArgumentException(string.Format("No gridObject with id {0} was found.", id));
        }

        [ContextMenu("Update Database")]
        public override void UpdateDatabase()
        {
            gridObjects = FindAssets().ToArray();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
