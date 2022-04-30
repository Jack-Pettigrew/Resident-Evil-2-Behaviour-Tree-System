using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;

namespace DD.Core.Items
{
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public bool CanInteract { set; get; }

        [field: SerializeField] public Item Item { private set; get; }
        [SerializeField] private int itemQuantity;

        public void SetItemQuantity(int quantity) => itemQuantity = quantity;

        public void Interact()
        {
            if(!CanInteract) return;

            Inventory.Instance.AddItem(Item, itemQuantity);
            Destroy(gameObject);
        }
    }
}
