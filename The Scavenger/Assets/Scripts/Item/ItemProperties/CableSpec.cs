using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Specifies a cables's transfer rate and which resource it can transport.
    /// </summary>
    public class CableSpec : ItemProperty
    {
        public override string Name => "Cable Spec";

        [field: SerializeField, Min(0)] public int TransferRate { get; private set; }

        [field: SerializeField] public ResourceType ResourceType { get; private set; }

        /// <summary>
        /// Adds the specified cable to a conduit.
        /// </summary>
        /// <param name="conduit">The conduit to add the cable to.</param>
        /// <returns>The added cable.</returns>
        public Cable AddCable(Conduit conduit)
        {
            Cable newCable;
            switch (ResourceType)
            {
                // TODO add fluid cable
                case ResourceType.Energy:
                    newCable = conduit.gameObject.AddComponent<EnergyCable>();
                    break;
                case ResourceType.Item:
                    newCable = conduit.gameObject.AddComponent<ItemCable>();
                    break;
                case ResourceType.Data:
                    newCable = conduit.gameObject.AddComponent<DataCable>();
                    break;
                default:
                    Debug.LogWarning("Cable type not implemented yet");
                    return null;
            }
            newCable.Spec = this;
            return newCable;
        }

        /// <summary>
        /// Converts the cables's ResourceType to the type it repesents.
        /// </summary>
        /// <returns>Type object representing the cable.</returns>
        public Type GetCableType()
        {
            switch (ResourceType)
            {
                // TODO add fluid cable
                case ResourceType.Energy:
                    return typeof(EnergyCable);
                case ResourceType.Item:
                    return typeof(ItemCable);
                case ResourceType.Data:
                    return typeof(DataCable);
                default:
                    Debug.LogWarning("Cable type not implemented yet");
                    return null;
            }
        }

    }
}
