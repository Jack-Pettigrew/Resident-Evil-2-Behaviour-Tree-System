using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;
using DD.Systems.InventorySystem;

namespace DD.Core
{
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData itemData;
        
        public void Interact()
        {
            Inventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
