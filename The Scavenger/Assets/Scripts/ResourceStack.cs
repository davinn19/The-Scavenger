using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO implement
namespace Scavenger
{
    public abstract class ResourceStack : RecipeComponent
    {
        public event Action Changed;

        [SerializeField] protected string ID;

        // Max amount things
        public abstract int DefaultMaxAmount { get; }
        private int maxAmount = -1;
        public int MaxAmount => maxAmount != -1 ? maxAmount : DefaultMaxAmount;
        
        public void SetMaxAmount(int maxAmount)
        {
            Debug.Assert(maxAmount > 0);
            Debug.Assert(this.maxAmount == -1); // Can only be set once
            this.maxAmount = maxAmount;
            SetAmount(amount);
        }

        // Amount things
        [SerializeField, Min(0)] protected int amount = 0;
        public int Amount => amount;
        public int GetAmountBeforeFull() => MaxAmount - Amount;
        public void AddAmount(int addBy) => SetAmount(amount + addBy);
        public void RemoveAmount(int removeBy) => SetAmount(amount - removeBy);
        public void SetAmount(int amount)
        {
            this.amount = Mathf.Clamp(amount, 0, MaxAmount);
            if (IsEmpty())
            {
                Clear();
            }
            else
            {
                Changed?.Invoke();
            }
        }
        
        /// <summary>
        /// Checks if the resourceStack contains any items.
        /// </summary>
        /// <returns>True if the itemStack is empty.</returns>
        public bool IsEmpty()
        {
            return amount == 0;
        }

        /// <summary>
        /// Checks if the resourceStack is full.
        /// </summary>
        /// <returns>True if the itemStack is full.</returns>
        public bool IsFull()
        {
            return amount == MaxAmount;
        }

        public virtual void Clear()
        {
            amount = 0;
            Changed?.Invoke();
        }

        protected void OnChange()
        {
            Changed?.Invoke();
        }

        /// <summary>
        /// Checks if another resource stack can substitute this in a recipe.
        /// </summary>
        /// <param name="other">The other stack to substitute with.</param>
        /// <returns>True if the other stack can be substituted.</returns>
        public virtual bool CanSubstituteWith(RecipeComponent other)
        {
            ResourceStack otherResourceStack = other as ResourceStack;
            if (otherResourceStack != null && otherResourceStack)   // other resource stack must contain items
            {
                return otherResourceStack.ID == ID;
            }
            return false;
        }

        /// <summary>
        /// A resourceStack without a definition is considered null.
        /// </summary>
        public static implicit operator bool(ResourceStack resourceStack)
        {
            return resourceStack.ID != null && resourceStack.ID != "";
        }
    }
}
