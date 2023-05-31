using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;
using DD.Core.Control;

namespace DD.Core.Items
{
    public class WorldItem : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public bool CanInteract { set; get; }

        [field: SerializeField] public ItemData Item { private set; get; }
        [SerializeField] private int itemQuantity;

        public void SetItemQuantity(int quantity) => itemQuantity = quantity;

        public void Interact(Interactor interactor)
        {
            Inventory.Instance.AddItem(Item, itemQuantity);
            Destroy(gameObject);
        }
    }
}
