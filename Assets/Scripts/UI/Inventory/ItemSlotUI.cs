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
        public Item Item { private set; get; }

        // UI COMPONENTS
        [SerializeField] private GameObject itemUI;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private RawImage itemImage;

        private ContextMenuUI contextMenu;

        private void Awake()
        {
            contextMenu = FindObjectOfType<ContextMenuUI>();
        }

        public void SetLinkedItem(Item item)
        {
            Item = item;
            UpdateSlot();
        }

        public void UpdateSlot()
        {
            if (Item == null)
            {
                itemUI.SetActive(false);
                return;
            }

            Debug.Log($"{Item.ItemData.name} : {Item.ItemAmount}");
            amountText.text = Item.ItemAmount.ToString();
            itemImage.texture = Item.ItemData.itemIcon;
            itemUI.SetActive(true);
        }

        public void RemoveItem()
        {
            itemUI.SetActive(false);
            Item = null;
            amountText.text = "";
            itemImage.texture = null;
        }

        public override void Select()
        {
            // Cancel if no Item
            if(Item == null) return;

            List<ContextMenuOption> contextMenuOptions = new List<ContextMenuOption>();

            // Equipment Items
            
            contextMenuOptions.Add(new ContextMenuOption("Use", () => { ItemUser.Instance.UseItem(Item.ItemData);}));
            contextMenuOptions.Add(new ContextMenuOption("Drop", () => {Inventory.Instance.DropItem(Item.ItemData);}));

            contextMenu.SetContextMenu(contextMenuOptions);
            contextMenu.SetPosition(transform.position);
            contextMenu.ShowContextMenu();
        }
    }
}
