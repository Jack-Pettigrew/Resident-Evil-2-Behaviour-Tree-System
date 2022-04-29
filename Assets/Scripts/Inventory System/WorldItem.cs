using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;

namespace DD.Core.Items
{
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public Item Item { private set; get; }
        [SerializeField] private int itemQuantity;

        public void SetItemQuantity(int quantity) => itemQuantity = quantity;

        public void Interact()
        {
            Inventory.Instance.AddItem(Item, itemQuantity);
            Destroy(gameObject);
        }
    }
}
