using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;

namespace DD.Core.Items
{
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private Item item;
        [SerializeField] private int itemQuantity;

        public void SetItemQuantity(int quantity) => itemQuantity = quantity;

        public void Interact()
        {
            Inventory.Instance.AddItem(item, itemQuantity);
            Destroy(gameObject);
        }
    }
}
