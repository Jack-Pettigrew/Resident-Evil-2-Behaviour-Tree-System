using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DD.Core.Control;
using DD.Systems.InventorySystem;

namespace DD.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Canvas inventoryCanvas;
        [SerializeField] private GameObject itemSlotUIPrefab;
        [SerializeField] private RectTransform itemGroup;

        public UnityEvent OnInventoryDisplayed;
        public UnityEvent OnInventoryHidden;

        private List<ItemSlotUI> uiItemSlots = new List<ItemSlotUI>();
                
        private void Start() {
            // Setup
            for (int i = 0; i < Inventory.Instance.MaxInventorySize; i++)
            {
                uiItemSlots.Add(Instantiate(itemSlotUIPrefab, itemGroup).GetComponent<ItemSlotUI>());
            }
            
            // Inventory UI Toggle
            InputManager.Instance.OnInventoryToggle += () => 
            {
                inventoryCanvas.enabled = !inventoryCanvas.enabled;

                if(inventoryCanvas.enabled)
                {
                    UpdateUI();
                    OnInventoryDisplayed?.Invoke();
                }
                else
                {
                    OnInventoryHidden?.Invoke();
                }
            };

            // Inventory Updated
            Inventory.Instance.OnInventoryUpdated += UpdateUI;

            // Inventory Added Item UI
            // Inventory.Instance.OnItemAdded += HandleAddedItemUI;
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
                    uiItemSlots[i].UpdateSlot();
                }
            }
        }
    }
}
