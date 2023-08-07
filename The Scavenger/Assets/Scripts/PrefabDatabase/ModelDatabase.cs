using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [CreateAssetMenu(fileName = "ModelDatabase", menuName = "Scavenger/Database/Model")]
    public class ModelDatabase : PrefabDatabase<Model>
    {
        [SerializeField] private Model[] models;

        public override Model Get(string id)
        {
            foreach (Model model in models)
            {
                if (model.name == id)
                {
                    return model;
                }
            }
            throw new ArgumentException(string.Format("No model with id {0} was found.", id));
        }

        [ContextMenu("Update Database")]
        protected override void UpdateDatabase()
        {
            models = FindAssets().ToArray();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
