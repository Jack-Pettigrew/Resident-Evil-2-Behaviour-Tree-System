using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DD.Core.InventorySystem;
using TMPro;

namespace DD.UI
{
    public class ItemSlotUI : MenuItem
    {
        // Item
        public ItemSlot ItemSlot { private set; get; }

        // UI COMPONENTS
        [SerializeField] private GameObject itemToggle;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private RawImage itemImage;

        private ContextMenuUI contextMenu;

        private void Awake()
        {
            contextMenu = FindObjectOfType<ContextMenuUI>();
        }

        /// <summary>
        /// Sets up the link between this UI slot the given ItemSlot. 
        /// </summary>
        /// <param name="itemSlot">An inventory item slot.</param>
        public void SetLinkedItemSlot(ItemSlot itemSlot)
        {
            ItemSlot = itemSlot;

            itemSlot.OnItemUpdated += UpdateUI;
            itemSlot.OnItemDepleted += RemoveItem;

            UpdateUI();
            itemImage.texture = itemSlot.ItemData.itemIcon;
            itemToggle.SetActive(true);
        }

        public void UpdateUI()
        {
            amountText.text = ItemSlot.Amount.ToString();
        }

        public void RemoveItem()
        {
            ItemSlot = null;
            itemToggle.SetActive(false);
            itemImage.texture = null;
        }

        public override void Select()
        {
            // Cancel if no Item
            if(ItemSlot == null)
            {
                return;
            }
            
            contextMenu.SetContextMenu(new ContextMenuOption[] {
                new ContextMenuOption("Use", ItemSlot.ItemData.Use),
                new ContextMenuOption("Drop", () => {Inventory.Instance.DropItem(ItemSlot.ItemData);})
            });

            contextMenu.SetPosition(transform.position);
            contextMenu.ShowContextMenu();
        }
    }
}
