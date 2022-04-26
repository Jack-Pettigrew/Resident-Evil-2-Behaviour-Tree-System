using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DD.Core.Items;
using DD.Systems.InventorySystem;
using TMPro;

namespace DD.UI
{
    public class ItemSlotUI : MenuItem
    {
        // Item
        public ItemSlot ItemSlot { private set; get; }

        // UI COMPONENTS
        [SerializeField] private GameObject itemUI;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private RawImage itemImage;

        private ContextMenuUI contextMenu;

        private void Awake()
        {
            contextMenu = FindObjectOfType<ContextMenuUI>();
        }

        public void SetLinkedItem(ItemSlot itemSlot)
        {
            ItemSlot = itemSlot;
        }

        public void UpdateSlot()
        {
            if (ItemSlot == null)
            {
                itemUI.SetActive(false);
                return;
            }

            amountText.text = ItemSlot.ItemQuantity.ToString();
            itemImage.texture = ItemSlot.Item.itemIcon;
            itemUI.SetActive(true);
        }

        public void RemoveItem()
        {
            itemUI.SetActive(false);
            ItemSlot = null;
            amountText.text = "";
            itemImage.texture = null;
        }

        public override void Select()
        {
            // Cancel if no ItemSlot
            if(ItemSlot == null) return;

            // Item Context Menu
            List<ContextMenuOption> contextMenuOptions = ItemSlot.Item.GetContextMenuOptions();

            if(contextMenuOptions.Count == 0) return;
            
            contextMenu.SetContextMenu(contextMenuOptions);
            contextMenu.SetPosition(transform.position);
            contextMenu.ShowContextMenu();
        }
    }
}
