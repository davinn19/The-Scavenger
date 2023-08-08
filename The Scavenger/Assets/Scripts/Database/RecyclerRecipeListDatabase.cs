using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [CreateAssetMenu(fileName = "RecyclerRecipeListDatabase", menuName = "Scavenger/Database/Recycler")]
    public class RecyclerRecipeListDatabase : PrefabDatabase<RecyclerRecipeList>
    {
        private const string recyclerRecipeListDirectory = "Assets/Prefabs/Recipes/Recycler";

        [SerializeField] private RecyclerRecipeList[] recipeLists;

        [ContextMenu("Generate Recipes")]
        public override void UpdateDatabase()
        {
            List<RecyclerRecipeList> newRecipeLists = new List<RecyclerRecipeList>();

            string[] subfolders = AssetDatabase.GetSubFolders(recyclerRecipeListDirectory);

            // Only keep the end folder
            for (int i = 0; i < subfolders.Length; i++)
            {
                string subfolder = subfolders[i];
                string[] path = subfolder.Split('/');
                subfolders[i] = path[path.Length - 1];
            }

            List<RecyclerRecipeList> foundRecipeLists = FindAssets();

            foreach (RecyclerRecipeList recipeList in foundRecipeLists)
            {
                bool tierExists = false;

                // Check if a folder exists for each existing recipe list, delete the rest
                foreach (string subfolder in subfolders)
                {
                    if (subfolder == recipeList.name)
                    {
                        recipeList.UpdateDatabase();
                        tierExists = true;
                        ArrayUtility.Remove(ref subfolders, subfolder);

                        newRecipeLists.Add(recipeList);
                        break;
                    }
                }

                if (!tierExists)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(recipeList));
                }
            }

            // Make new recipe lists for unpaired subfolders
            foreach (string subfolder in subfolders)
            {
                RecyclerRecipeList recipeList = CreateInstance<RecyclerRecipeList>();
                AssetDatabase.CreateAsset(recipeList, recyclerRecipeListDirectory + "/" + subfolder + ".asset");
                recipeList.name = subfolder;
                recipeList.UpdateDatabase();

                newRecipeLists.Add(recipeList);
            }

            recipeLists = newRecipeLists.ToArray();
            AssetDatabase.SaveAssets();
        }
    }
}
