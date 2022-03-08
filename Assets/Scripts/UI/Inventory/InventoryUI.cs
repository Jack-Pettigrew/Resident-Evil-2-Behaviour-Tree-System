using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.InventorySystem;

namespace DD.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemSlotUIPrefab;
        [SerializeField] private RectTransform itemGroup;

        private Queue<ItemSlotUI> uiItemSlots = new Queue<ItemSlotUI>();
        
        private void Start() {
            // Setup
            for (int i = 0; i < Inventory.Instance.MaxInventorySize; i++)
            {
                uiItemSlots.Enqueue(Instantiate(itemSlotUIPrefab, itemGroup).GetComponent<ItemSlotUI>());
            }
            
            // Inventory Events Subbing
            Inventory.Instance.OnNewInventoryItem += HandleNewItem;
        }

        private void HandleNewItem(ItemSlot itemSlot)
        {
            // Add item to next empty ui slot
            foreach (ItemSlotUI uiItemSlot in uiItemSlots)
            {
                if(uiItemSlot.ItemSlot == null)
                {
                    uiItemSlot.SetLinkedItemSlot(itemSlot);
                    return;
                }
            }
        }
    }
}
