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
        // Downcasting Code Smell alert but thanks to the polymorphism and that not all Items being 'usable'.
        // A Use function in class Item would force empty default implementations where not necessary.
        
        // Switch Type Checks: https://stackoverflow.com/questions/298976/is-there-a-better-alternative-than-this-to-switch-on-type
        switch (item)
        {
            case HealingItemData healingItem:
                playerHealth.Heal(healingItem.healAmount);
                Inventory.Instance.RemoveItem(item, 1);
                break;

            default:
                break;
        }
        
        // Can't 'use' ammo
    }
}

// ItemScriptableObject - abstract Item CreateItemInstance
/* 
    WorldItem returns the result of that function to the Inventory.Instance.AddItem method,
    removing the need for the inventory to create an Item instance.
    This allows for Item to be purely abstract as Inventory no longer creates instances of Item. The ScriptableObject is
    responsible for it's own factory.
*/

// abstract Item - virutal GetContextOptions (Default: Drop)
// UsableItem : IUsable
// QuestItem : Item