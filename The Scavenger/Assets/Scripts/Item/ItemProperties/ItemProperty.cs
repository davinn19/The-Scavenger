using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Scavenger
{
    public class ItemProperty
    {
        public ItemPropertyDefinition definition;

        public T SetDefinition<T>() where T : ItemPropertyDefinition, new()
        {
            T newDefinition = new T();
            definition = newDefinition;
            return newDefinition;
        }

        public ItemPropertyDefinition SetDefinition(Type newDefinition)  // https://learn.microsoft.com/en-us/dotnet/api/system.type.getconstructor?view=net-7.0
        {
            ItemPropertyDefinition definitionInstance = newDefinition.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.ExplicitThis,
                new Type[0],
                new ParameterModifier[0]
                )
                .Invoke(new object[0]) as ItemPropertyDefinition;

            definition = definitionInstance;
            return definitionInstance;
        }


        public bool IsDefinedBy(Type definitionType)
        {
            return definition.GetType() == definitionType;
        }
    }

    public abstract class ItemPropertyDefinition
    {
        public static List<Type> GetDefinitions()
        {
            List<Type> types = new List<Type>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(ItemPropertyDefinition)))
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
