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

            EquipmentManager.Instance.EquipWeapon(weaponSlot, selectedWeapon);
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


// Menu menus should record their active status to the menu manager.
// MenuManager closes active menus if button is pressed (calls IClosable which runs logic for cancelling menus in process)

// WeaponItem::Use() => WeaponSlotPickerUI::SelectWeaponForEquip(WeaponItem)
// SelectWeaponForEquip:
// - Opens weapon slot picker UI of which each of the weapon slots ui objects calls WeaponSlotPickerUI::SelectWeaponSlot(enum WeaponSlot)
// 
// SelectWeaponSlot:
// - Calls EquipmentManager::EquipWeapon(WeaponSlot, WeaponItem)
// - Closes weapon slot picker UI
