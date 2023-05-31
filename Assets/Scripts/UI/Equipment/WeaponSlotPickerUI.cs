using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;
using DD.Core.Combat;

namespace DD.UI
{
    public class WeaponSlotPickerUI : MonoBehaviour, ICancelable
    {
        [SerializeField] private RectTransform weaponSlotsParent;
        private EquipmentItem selectedWeapon;

        public void SelectWeaponForEquip(EquipmentItem weaponItem)
        {
            selectedWeapon = weaponItem;
            weaponSlotsParent.gameObject.SetActive(true);
        }

        public void SelectEquipWeapon(WeaponSlot weaponSlot)
        {
            if(selectedWeapon == null) return;

            EquipmentManager.Instance.EquipWeapon(selectedWeapon, weaponSlot);
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
