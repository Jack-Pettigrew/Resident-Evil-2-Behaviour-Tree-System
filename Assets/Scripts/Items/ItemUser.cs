using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core;
using DD.Core.Items;
using DD.Core.Combat;
using DD.Systems.InventorySystem;

public class ItemUser : MonoBehaviour
{
    // Singleton
    public static ItemUser Instance { private set; get; }

    [Header("Components")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private EquipmentManager equipmentManager;

    private void Awake() {
        Instance = this;
    }

    public void SetPlayerHealthComponent(Health healthComponent) => playerHealth = healthComponent;
    public void SetEquipmentManagerComponent(EquipmentManager equipmentManager) => this.equipmentManager = equipmentManager;

    /// <summary>
    /// Handles the using of the given Item given it's class type.
    /// </summary>
    /// <param name="item"></param>
    public void UseItem(ItemData item)
    {
        if(item is HealingItem)
        {
            HealingItem healingItem = (HealingItem)item;
            playerHealth.Heal(healingItem.healAmount);
            Inventory.Instance.ReduceItem(item, 1);
            return;
        }

        if(item is AmmoItem)
        {
            // Does this handle Context Menu using or Reload using?
            // Makes sense to make it generic enough to cater to both

            // What does each care about?
            // Inventory UI cares about combining ammo item with weapon of a similar type
            // Reloading cares about finding ammo for the currently active weapon and consuming as much as it can
        }
    }
}
