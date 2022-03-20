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
            Debug.LogError($"ItemSlotUI {name} has no Select method logic");
        }
    }
}
