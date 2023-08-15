using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;
using DD.Core.Control;
using System;

namespace DD.Core.Items
{
    public class WorldItem : MonoBehaviour, IInteractable
    {        
        [field: SerializeField] public bool CanInteract { set; get; }

        [field: SerializeField] public ItemData ItemData { private set; get; }
        [field: SerializeField] public Vector3 InteractionIconOffset { set; get; }

        [SerializeField] private int itemQuantity;

        public void SetItemQuantity(int quantity) => itemQuantity = quantity;

        public void Interact(Interactor interactor)
        {
            Inventory.Instance.AddItem(ItemData, itemQuantity);
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + InteractionIconOffset, .1f);
        }
    }
}
