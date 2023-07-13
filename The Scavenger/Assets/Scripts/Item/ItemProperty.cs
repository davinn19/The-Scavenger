using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Scavenger
{
    [Serializable]
    public abstract class ItemProperty : ScriptableObject
    {
        public abstract string Name { get; }
        public Item Item { get; set; }

        public static List<Type> GetProperties()
        {
            List<Type> types = new List<Type>();

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
