using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Items
{
    public abstract class ItemData : ScriptableObject
    {
        [Header("Base Info")]
        public string itemName;
        [TextArea] public string itemDescription;
        public WorldItem itemPrefab;
        public Texture itemIcon;

        [Header("Inventory Settings")]
        public bool isStackable = true;
        public int maxStackSize = 10;
        public bool isDroppable = true;
    }
}
