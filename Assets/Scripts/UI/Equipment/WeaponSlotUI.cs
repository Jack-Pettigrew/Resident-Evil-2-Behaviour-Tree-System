using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DD.Core.Items;
using DD.Core.Combat;

namespace DD.UI
{
    public class WeaponSlotUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private WeaponSlot associatedWeaponSlot;

        [Header("UI Components")]
        [SerializeField] private WeaponSlotPickerUI weaponSlotPickerUI;
        [SerializeField] private RawImage iconComponent;

        private void OnEnable() {
            if(!EquipmentManager.Instance) return;
            
            DrawWeaponSlot();
        }

        public void DrawWeaponSlot()
        {
            Weapon weapon = EquipmentManager.Instance.WeaponSlots[(int)associatedWeaponSlot];

            if(!weapon) 
            {
                iconComponent.texture = null;
                iconComponent.enabled = false;
                return;
            }
            
            Item item = weapon.WorldItem.Item;

            if(item)
            {
                iconComponent.texture = item.itemIcon;
                iconComponent.enabled = true;

            }
            else
            {
                iconComponent.texture = null;
                iconComponent.enabled = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {            
            weaponSlotPickerUI.SelectEquipWeapon(associatedWeaponSlot);
        }
    }
}