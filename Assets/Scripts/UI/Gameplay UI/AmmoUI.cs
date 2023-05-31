using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DD.Core.Combat;
using DD.Core.Items;
using DD.Systems.InventorySystem;

namespace DD.UI
{
    public class AmmoUI : GameplayUI
    {        
        [SerializeField] private TextMeshProUGUI currentAmmoCounter;
        [SerializeField] private TextMeshProUGUI remainingAmmoCounter;
                
        private void Start() {
            // Subscribe to weapon swapping
            EquipmentManager.Instance.OnWeaponSwapped += SubscribeToGun;
            EquipmentManager.Instance.OnWeaponSwapped += UpdateUI;

            // Subscribe to Inventory adding Ammo checking
            Inventory.Instance.OnItemAdded += CheckAddedItem;
            
            // Initial subscribing and update
            SubscribeToGun();
            UpdateUI();
        }

        public void CheckAddedItem(ItemSlot itemSlot)
        {
            if(itemSlot.ItemData is AmmoItem)
            {
                UpdateUI();
            }
        }

        /// <summary>
        /// Subscribes the UI to the currently active Gun
        /// </summary>
        public void SubscribeToGun()
        {
            if(EquipmentManager.Instance.ActiveWeapon is Gun)
            {
                Gun gun = (Gun) EquipmentManager.Instance.ActiveWeapon;
                gun.OnShot += UpdateUI;
                gun.OnReloaded += UpdateUI;
            }
        }
        
        /// <summary>
        /// Updates the current UI
        /// </summary>
        public override void UpdateUI()
        {
            if(EquipmentManager.Instance.ActiveWeapon is Gun)
            {
                Gun gun = (Gun) EquipmentManager.Instance.ActiveWeapon;
                currentAmmoCounter.text = gun.CurrentAmmo.ToString();

                ItemSlot ammoSlot = Inventory.Instance.FindItemSlot(gun.GunAmmoItem);
                remainingAmmoCounter.text = ammoSlot != null ? ammoSlot.ItemQuantity.ToString() : "0";


                // Fade In
                if(!IsVisible())
                {
                    StartFadeTransition(0, 1, fadeTime);
                }
            }
            else if(IsVisible())
            {
                // Fade out
                StartFadeTransition(1, 0, fadeTime);
            }
        }
    }
}
