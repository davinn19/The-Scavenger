using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class CableSpec : ItemProperty
    {
        public override string Name => "Cable Spec";

        [field: SerializeField, Min(0)] public int TransferRate { get; private set; }

        [field: SerializeField] public ResourceType ResourceType { get; private set; }

        public Cable AddCable(Conduit conduit)
        {
            Cable newCable;
            switch (ResourceType)
            {
                // TODO add other cables
                case ResourceType.Energy:
                    newCable = conduit.gameObject.AddComponent<EnergyCable>();
                    break;
                case ResourceType.Item:
                    newCable = conduit.gameObject.AddComponent<ItemCable>();
                    break;
                default:
                    Debug.LogWarning("Cable type not implemented yet");
                    return null;
            }
            newCable.spec = this;
            return newCable;
        }

        public Type GetCableType()
        {
            switch (ResourceType)
            {
                // TODO add other cables
                case ResourceType.Energy:
                    return typeof(EnergyCable);
                case ResourceType.Item:
                    return typeof(ItemCable);
                default:
                    Debug.LogWarning("Cable type not implemented yet");
                    return null;
            }
        }

    }
}
