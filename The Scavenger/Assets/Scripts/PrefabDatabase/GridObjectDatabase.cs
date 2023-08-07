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

        public override GridObject Get(string id)
        {
            GridObject cacheResult = base.Get(id);
            if (cacheResult)
            {
                return cacheResult;
            }

            foreach (GridObject gridObject in gridObjects)
            {
                if (gridObject.name == id)
                {
                    AddToCache(gridObject);
                    return gridObject;
                }
            }
            throw new ArgumentException(string.Format("No gridObject with id {0} was found.", id));
        }

        [ContextMenu("Update Database")]
        protected override void UpdateDatabase()
        {
            gridObjects = FindAssets().ToArray();

            foreach (GridObject gridObject in gridObjects)
            {
                Debug.Log(gridObject.name);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
