using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Scavenger
{
    [Serializable]
    public class ItemProperty : ScriptableObject
    {
        public ItemPropertyDefinition definition;

        public ItemPropertyDefinition InitDefinition(Type newDefinition)  // https://learn.microsoft.com/en-us/dotnet/api/system.type.getconstructor?view=net-7.0
        {
            if (definition != null)
            {
                return null;
            }
            definition = CreateInstance(newDefinition) as ItemPropertyDefinition;

            Debug.Log(definition);
            return definition;
        }


        public bool IsDefinedBy(Type definitionType)
        {
            if (definition == null)
            {
                Debug.LogError("definition is null");
                return false;
            }

            return definition.GetType() == definitionType;
        }
    }


    [Serializable]
    public abstract class ItemPropertyDefinition : ScriptableObject
    {
        public static List<Type> GetDefinitions()
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
                    if (type.IsSubclassOf(typeof(ItemPropertyDefinition)) && !type.IsAbstract)
                    {
                        types.Add(type);
                    }
                }
            }
            return types;
        }

        public abstract string Name { get; }
    }

}
