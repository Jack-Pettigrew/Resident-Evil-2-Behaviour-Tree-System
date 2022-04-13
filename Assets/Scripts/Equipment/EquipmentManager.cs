using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;

namespace DD.Core.Combat
{
    public class EquipmentManager : MonoBehaviour
    {        
        // Equipment Slots
        private Weapon[] equipmentSlots = new Weapon[4];

        // Active Equipment
        private int activeWeaponSlotID = 0;
        public Weapon ActiveWeapon { get { return equipmentSlots[activeWeaponSlotID]; } }

        // Animation
        [SerializeField] private Animator playerAnimator;

        // Events
        public event Action<int> OnWeaponSwap;

        private void Start() {
            // ***** TEST *****
            int i = 0;
            foreach (var gun in FindObjectsOfType<Gun>())
            {
                equipmentSlots[i] = gun;
                gun.SetCanUse(true);
                i++;
            }

            // Input Swap
            InputManager.Instance.OnQuickSlotChange += SwapWeapon;
            InputManager.Instance.OnShoot += UseWeapon;
            InputManager.Instance.OnReload += ReloadWeapon;
        }

        public void UseWeapon()
        {
            ActiveWeapon.Attack();
        }
        
        /// <summary>
        /// Equips the given weapon in the associated equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the equipment slot to set the weapon to.</param>
        /// <param name="weaponToEquip">Weapon to equip.</param>
        public void EquipWeapon(int equipmentSlotID, Weapon weaponToEquip)
        {
            if (equipmentSlotID < 0 || equipmentSlotID >= equipmentSlots.Length)
            {
                Debug.LogWarning("Equipment Slot doesn't exist. The weapon wasn't equipped.");
                return;
            }

            equipmentSlots[equipmentSlotID] = weaponToEquip;

            if(equipmentSlotID == activeWeaponSlotID)
            {
                SwapWeapon(equipmentSlotID);
            }
        }

        public void ReloadWeapon()
        {
            if(ActiveWeapon.WeaponType == WeaponType.Gun)
            {
                Gun gun = ActiveWeapon as Gun;
                gun.Reload(gun.MaxAmmoCapacity);
            }
        }

        /// <summary>
        /// Unequips the weapon associated with the equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the slot to unequip.</param>
        public void UnequipWeaponFromSlot(int equipmentSlotID)
        {
            if(equipmentSlotID < 0 || equipmentSlotID >= equipmentSlots.Length)
            {
                Debug.LogWarning("Equipment Slot doesn't exist. The weapon wasn't unequipped.");
                return;
            }
            
            equipmentSlots[equipmentSlotID] = null;

            if(equipmentSlotID == activeWeaponSlotID)
            {
                SwapWeapon(equipmentSlotID);
            }
        }

        /// <summary>
        /// Swaps the active equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the slot to make active.</param>
        public void SwapWeapon(int equipmentSlotID)
        {
            if(equipmentSlotID < 0 || equipmentSlotID >= equipmentSlots.Length)
            {
                Debug.LogWarning("Equipment Slot doesn't exist. The weapon swapped equipped.");
                return;
            }

            // d-pad/1-4 switches the currently active equipment to the selected one
            if(ActiveWeapon != null)
            {
                // Unequip current weapon
                ActiveWeapon.isEquipped = false;
                ActiveWeapon.SetCanUse(false);

                // Animate weapon swap with gameobject hide/move callback on complete (possibly WeaponSlot class as helper?)

                // **** TEST SWAP ****
                ActiveWeapon.gameObject.SetActive(false);
            }
            
            // switch the active equipment slot
            activeWeaponSlotID = equipmentSlotID;

            // If weapon occupies this slot...
            if(ActiveWeapon != null)
            {
                // Set weapon equiped
                ActiveWeapon.isEquipped = true;
                ActiveWeapon.SetCanUse(true);

                // Animate weapon swap with gameobject hide/move callback on complete (possibly WeaponSlot class as helper?)
                // Update animator with appropriate weapon hold pose (including idle if no weapon)
                
                // **** TEST SWAP ****
                ActiveWeapon.gameObject.SetActive(true);
            }
            // ...else ensure default anim + settings are being used
            else
            {
                // ensure default
            }

            // (torso animations may need it's own state flow to ensure correct animations are selected e.g. gun idle, to aiming, to swap, to new gun pose, to disabled torso layer, to enabled torso layer)

            // ui temporarily appears when swapping - via event
            OnWeaponSwap?.Invoke(activeWeaponSlotID);
        }

        // TODO:
        // change loadout from inventory
        // input - swap active equipment
        // input - shoot?? (maybe shoot should be it's own class)
    }
}
