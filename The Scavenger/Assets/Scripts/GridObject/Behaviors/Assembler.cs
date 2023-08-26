using Scavenger.Recipes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs, implement
    [RequireComponent(typeof(AssemblerItemBuffer), typeof(EnergyBuffer))]
    public class Assembler : GridObjectBehavior
    {
        private AssemblerItemBuffer itemBuffer;
        private EnergyBuffer energyBuffer;

        private AssemblerRecipe currentRecipe;

        [field: SerializeField, Min(1)]
        public int ProgressPerBuild { get; private set; }
        public int CurrentProgress { get; private set; }

        [SerializeField, Min(0)]
        private int energyPerTick;

        protected override void Init()
        {
            base.Init();
            energyBuffer = GetComponent<EnergyBuffer>();
            itemBuffer = GetComponent<AssemblerItemBuffer>();

            foreach (ItemStack input in itemBuffer.GetInputSlots())
            {
                input.Changed += UpdateCurrentRecipe;
            }
        }

        protected override void TickUpdate()
        {
            energyBuffer.ResetEnergyTransferCount();

            // Resets if there is no valid recipe
            if (currentRecipe == null)
            {
                CurrentProgress = 0;
                return;
            }

            // Pauses if output cannot be inserted
            ItemStack output = currentRecipe.output.Clone();
            bool outputMoved = output.Amount == ItemTransfer.MoveStackToStack(output, itemBuffer.GetOutput(), output.Amount, itemBuffer.AcceptsItemStack, true);
            if (!outputMoved)
            {
                return;
            }

            // Pauses if not enough energy
            if (energyBuffer.Energy < energyPerTick)
            {
                return;
            }

            // Do progress
            CurrentProgress++;
            energyBuffer.Spend(energyPerTick);

            // Stop here if progress is not completed yet
            if (CurrentProgress < ProgressPerBuild)
            {
                return;
            }

            // Otherwise, do the recipe
            // Preserve any old persistent data first (TODO TEST)
            output.SetPersistentData(itemBuffer.GetBase().PersistentData);

            ItemTransfer.MoveStackToStack(output, itemBuffer.GetOutput(), output.Amount, itemBuffer.AcceptsItemStack, false);
            itemBuffer.SpendInputs();
        }

        private void UpdateCurrentRecipe()
        {
            AssemblerRecipe newRecipe = AssemblerRecipes.Instance.GetRecipeWithInput(itemBuffer.GetBase(), itemBuffer.GetAddons());

            if (newRecipe != currentRecipe)
            {
                currentRecipe = newRecipe;
                CurrentProgress = 0;
            }
        }
    }
}
