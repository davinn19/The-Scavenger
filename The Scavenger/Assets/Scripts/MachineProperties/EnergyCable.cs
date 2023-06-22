using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class EnergyCable : ResourceTransfer
    {


        public override void InitConnectedSides()
        {
            GridObject gridObject = GetComponent<GridObject>();

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                GridObject adjObject = gridObject.GetAdjacentObject(side);

                if (!adjObject)
                {
                    continue;
                }

                Conduit adjConduit;

                if (adjObject.TryGetComponent(out adjConduit))
                {
                    SetConnected(side, true);
                    adjConduit.SetConnected(side * -1, true);   // TODO move to updating
                    continue;
                }

                ConduitInterface adjInterface;

                if (adjObject.TryGetComponent(out adjInterface))
                {
                    SetConnected(side, true);
                }

            }
        }

        public override bool CanConnectTo(GridObject gridObject)
        {
            throw new System.NotImplementedException();
        }

    }
}
