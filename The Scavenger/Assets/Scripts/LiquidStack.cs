using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO implement, add docs
namespace Scavenger
{
    public class LiquidStack : ResourceStack
    {
        public override int DefaultMaxAmount => 1000;
        public Liquid Liquid;   // TODO create liquid database

        public LiquidStack()
        {
            ID = "";
            amount = 0;
        }

        public LiquidStack(Liquid liquid, int amount)
        {
            this.amount = amount;
            ID = liquid.name;
        }
    }
}
