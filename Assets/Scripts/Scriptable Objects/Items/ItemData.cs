using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.UI;
using DD.Systems.InventorySystem;

namespace DD.Core.Items
{
    public abstract class ItemData : ScriptableObject
    {
        [Header("Database Info")]
        [SerializeField, ReadOnly] private int ItemID;

        [Header("Base Info")]
        public string itemName;
        [TextArea] public string itemDescription;
        public WorldItem itemPrefab;
        public Texture itemIcon;

        [Header("Inventory Settings")]
        public bool isStackable = true;
        public int maxStackSize = 10;
        public bool isDroppable = true;
        public bool isCombinable = false;

        public void SetItemID(int id) => ItemID = id;

        public abstract Item CreateItemInstance(int amountOfItem);
    }
}
