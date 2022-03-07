using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Base Info")]
        public string itemName;
        [TextArea] public string itemDescription;
        public GameObject itemPrefab;
        public Texture itemIcon;

        [Header("Inventory Settings")]
        public bool isStackable = true;
        public int maxStackSize = 10;        
    }
}
