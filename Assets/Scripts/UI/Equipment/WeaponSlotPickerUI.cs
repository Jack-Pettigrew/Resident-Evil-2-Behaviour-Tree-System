using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;
using DD.Core.Combat;

namespace DD.UI
{
    public class WeaponSlotPickerUI : MonoBehaviour, ICancelable
    {
        public static WeaponSlotPickerUI Instance;
        
        [SerializeField] private RectTransform weaponSlotsParent;
        private EquipmentItem selectedWeapon;

        private void Awake() {
            Instance = this;
        }

        public void SelectWeaponForEquip(EquipmentItem weaponItem)
        {
            selectedWeapon = weaponItem;
            weaponSlotsParent.gameObject.SetActive(true);
        }

        public void SelectEquipWeapon(WeaponSlot weaponSlot)
        {
            if(selectedWeapon == null) return;

            EquipmentManager.Instance.EquipWeaponToSlot(selectedWeapon, weaponSlot);
            Cancel();
        }

        public void Cancel()
        {
            selectedWeapon = null;
            weaponSlotsParent.gameObject.SetActive(false);
        }
    }

    public interface ICancelable
    {
        void Cancel();
    }
}
