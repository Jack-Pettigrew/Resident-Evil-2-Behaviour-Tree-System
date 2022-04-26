using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.UI;
using DD.Systems.InventorySystem;

namespace DD.Core.Items
{   
    public abstract class Item : ScriptableObject
    {
        [Header("Item Info")]
        [ReadOnly] public int itemID;
        public string itemName;
        [TextArea] public string itemDescription;
        public Texture itemIcon;
        public WorldItem itemPrefab;

        [Header("Inventory Settings")]
        public bool isStackable;
        [Tooltip("Controls whether an Item is droppable from the Inventory. Drop context menu option is automatically included.")]
        public bool isDroppable;
        [field: SerializeField] private ContextMenuOption[] contextMenuOptions;

        public void SetItemID(int id) => itemID = id;

        public abstract void Use();

        public List<ContextMenuOption> GetContextMenuOptions()
        {
            // Make new list so we don't pile on Drop buttons
            List<ContextMenuOption> options = new List<ContextMenuOption>(contextMenuOptions);

            if(isDroppable)
            {
                options.Add(new ContextMenuOption("Drop", () => Inventory.Instance.DropItem(this)));
            }
            
            return options;
        }
    }
}

// Next try adding derrived items