using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;
using DD.Systems.InventorySystem;

namespace DD.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemSlotUIPrefab;
        [SerializeField] private RectTransform itemGroup;

        private List<ItemSlotUI> uiItemSlots = new List<ItemSlotUI>();
        
        private void Start() {
            // Setup
            for (int i = 0; i < Inventory.Instance.MaxInventorySize; i++)
            {
                uiItemSlots.Add(Instantiate(itemSlotUIPrefab, itemGroup).GetComponent<ItemSlotUI>());
            }

            // Inventory Updated
            Inventory.Instance.OnInvetoryUpdated += UpdateUI;

            // Inventory Added Item UI
            // Inventory.Instance.OnItemAdded += HandleAddedItemUI;
        }

        private void OnEnable() {
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Goes through the inventory and populates instantiated itemslotUIs with the relevant information

            for (int i = 0; i < uiItemSlots.Count; i++)
            {
                if(i >= Inventory.Instance.inventory.Count)
                {
                    uiItemSlots[i].RemoveItem();
                }
                else {
                    uiItemSlots[i].SetLinkedItem(Inventory.Instance.inventory[i]);
                }
            }
        }
    }
}
