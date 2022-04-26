using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.UI;

namespace DD.Core.Items
{   
    [System.Serializable]
    public abstract class Item : ICombinable
    {
        [field: SerializeField] public ItemData ItemData { private set; get; }
        public int ItemAmount { private set; get; }

        public Item(ItemData itemData, int amountOfItem)
        {
            ItemData = itemData;
            ItemAmount = amountOfItem;
        }

        /// <summary>
        /// Adds the amount to this Item.
        /// </summary>
        /// <param name="amountToAdd">Amount to add to this Item.</param>
        /// <returns>Amount actually added to Item.</returns>
        public int AddItemAmount(int amountToAdd)
        {
            if(amountToAdd < 0) return 0;

            // Calculate difference
            int toAddedDifference = ItemData.maxStackSize - (ItemAmount + amountToAdd);

            // If it's more than we can - remove surplus from adding amount
            if(toAddedDifference < 0)
            {
                amountToAdd += toAddedDifference;
            }
            
            ItemAmount += amountToAdd;

            return amountToAdd;
        }

        /// <summary>
        /// Removes the amount from this Item.
        /// </summary>
        /// <param name="amountToRemove">Amount to remove from this Item.</param>
        /// <returns>Amount actually removed from this Item.</returns>
        public int RemoveItemAmount(int amountToRemove)
        {
            if(amountToRemove < 0) return 0;

            int reducedAmount =  ItemAmount - amountToRemove;

            if(reducedAmount < 0)
            {
                amountToRemove += -reducedAmount;
            }

            ItemAmount -= amountToRemove;

            return amountToRemove;
        }

        public abstract void Use();
        public abstract bool Combine(Item combiningItem);

        public abstract List<ContextMenuOption> GetContextOptions();
    }

    public interface ICombinable
    {
        public abstract bool Combine(Item combiningItem);
    }
}
