using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private GameObject[] gameElements;

        private void Awake()
        {
            

            ItemDatabase.Load();
            GetComponent<RecipeLoader>().LoadRecipes();

            //game.SetActive(true);
        }

    }
}
