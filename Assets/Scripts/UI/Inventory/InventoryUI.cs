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
        public bool IsDisplaying { private set; get; } = false;
        
        [SerializeField] private Canvas inventoryCanvas;
        [SerializeField] private GameObject itemSlotUIPrefab;
        [SerializeField] private RectTransform itemGroup;

        public UnityEvent OnInventoryDisplayed;
        public UnityEvent OnInventoryHidden;

        private List<ItemSlotUI> uiItemSlots = new List<ItemSlotUI>();

        private void OnEnable() {           
            // UI Toggle
            GameManager.OnGamePause += HideMenu;
            InputManager.Instance.OnInventoryPressed += ToggleDisplay;

            // Inventory Updated
            Inventory.Instance.OnInventoryUpdated += UpdateUI;

            // Inventory Added Item UI
            // Inventory.Instance.OnItemAdded += HandleAddedItemUI;
        }

        private void OnDisable()
        {
            // UI Toggle
            GameManager.OnGamePause -= HideMenu;
        }

        private void Start()
        {
            // Setup
            for (int i = 0; i < Inventory.Instance.MaxInventorySize; i++)
            {
                uiItemSlots.Add(Instantiate(itemSlotUIPrefab, itemGroup).GetComponent<ItemSlotUI>());
            }
        }

        private void UpdateUI()
        {
            // Goes through the inventory and populates instantiated itemslotUIs with the relevant information

            for (int i = 0; i < uiItemSlots.Count; i++)
            {
                if (i >= Inventory.Instance.inventory.Count)
                {
                    uiItemSlots[i].RemoveItem();
                }
                else
                {
                    uiItemSlots[i].SetLinkedItem(Inventory.Instance.inventory[i]);
                    uiItemSlots[i].UpdateSlot();
                }
            }
        }
        private void ToggleDisplay()
        {
            IsDisplaying = !IsDisplaying;

            ToggleDisplay(IsDisplaying);
        }

        private void ToggleDisplay(bool toggle = false)
        {
            if (toggle)
            {
                ShowMenu();
            }
            else
            {
                HideMenu();
            }
        }

        private void HideMenu()
        {
            IsDisplaying = false;
            inventoryCanvas.enabled = false;
            InputManager.Instance.CursorToggle(false);
            OnInventoryHidden?.Invoke();
        }

        private void ShowMenu()
        {            
            IsDisplaying = true;
            UpdateUI();
            inventoryCanvas.enabled = true;
            InputManager.Instance.CursorToggle(true);
            OnInventoryDisplayed?.Invoke();
        }
    }
}
