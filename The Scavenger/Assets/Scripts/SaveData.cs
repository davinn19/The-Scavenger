using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Save
{
    [System.Serializable]
    public class GameSaveData
    {
        public MapSaveData MapSaveData;
        public InventorySaveData InventorySaveData;
    }

    [System.Serializable]
    public class MapSaveData
    {

    }

    [System.Serializable]
    public class InventorySaveData
    {
        public ItemStack[] InventoryItems;
        public ItemStack HeldItem;
    }

    [System.Serializable]
    public class GridObjectSaveData
    { 
        
    }


}
