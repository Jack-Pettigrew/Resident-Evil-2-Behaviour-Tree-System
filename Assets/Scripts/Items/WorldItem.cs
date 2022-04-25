using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;
using DD.Systems.InventorySystem;

namespace DD.Core.Items
{
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData itemData;
        [field: SerializeField] public int ItemAmount { private set; get; }

        public void SetItemAmount(int itemAmount)
        {
            if(itemAmount <= 0)
            {
                itemAmount = 1;
            }
            
            ItemAmount = Mathf.Min(itemAmount, itemData.maxStackSize);
        }

        public void Interact()
        {
            Item item = itemData.CreateItemInstance(ItemAmount);
            Inventory.Instance.AddItem(item);
            Destroy(gameObject);
        }

        private void OnValidate() {
            SetItemAmount(ItemAmount);
        }
    }
}
