using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Adds new behavior to the item it is attached to.
    /// </summary>
    [Serializable]
    public abstract class ItemProperty : ScriptableObject
    {
        public abstract string Name { get; }
        public Item Item { get; set; }

        /// <summary>
        /// Gets all item properties.
        /// </summary>
        /// <returns>List of all item properties.</returns>
        public static List<Type> GetProperties()
        {
            List<Type> types = new();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.IsSubclassOf(typeof(ItemProperty)) && !type.IsAbstract)
                    {
                        types.Add(type);
                    }
                }
            }
            return types;
        }
    }
}
