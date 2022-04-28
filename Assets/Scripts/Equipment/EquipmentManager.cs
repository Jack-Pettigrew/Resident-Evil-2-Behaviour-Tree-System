using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.Core.Items;
using UnityEngine.Animations.Rigging;

namespace DD.Core.Combat
{
    public class EquipmentManager : MonoBehaviour
    {        
        // Equipment Slots
        private Weapon[] weaponSlots = new Weapon[4];
        private int activeWeaponSlotID = 0;
        public Weapon ActiveWeapon { get { return weaponSlots[activeWeaponSlotID]; } }

        [Header("Animation")]
        [SerializeField] private Transform weaponHoldTransform;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private MultiAimConstraint aimTargetConstraint;

        public EquipmentItem testWeapon;

        // Events
        public event Action<int> OnWeaponSwap;

        private void Start() {
            // ***** TEST *****
            EquipWeapon(WeaponSlot.One, testWeapon);

            // Input Swap
            InputManager.Instance.OnQuickSlotChange += SwapWeapon;
            InputManager.Instance.OnShoot += UseWeapon;
            InputManager.Instance.OnReload += ReloadWeapon;
        }

        private void LateUpdate() {
            UpdateAnimations();
        }     

        public void UseWeapon()
        {
            ActiveWeapon.Attack();
        }
        
        public void ReloadWeapon()
        {
            // if(ActiveWeapon.WeaponType == WeaponType.Gun)
            // {
            //     Gun gun = ActiveWeapon as Gun;
            //     gun.Reload(gun.MaxAmmoCapacity);
            // }
        }

        /// <summary>
        /// Equips the given weapon in the associated equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the equipment slot to set the weapon to.</param>
        /// <param name="weaponToEquip">Weapon to equip.</param>
        public void EquipWeapon(WeaponSlot weaponSlot, EquipmentItem equipmentItem)
        {
            int weaponSlotID = (int) weaponSlot;

            // Remove previous Weapon
            GameObject previousWeapon = weaponSlots[weaponSlotID]?.GetComponent<GameObject>();
            if(!previousWeapon)
            {
                Destroy(previousWeapon);
            }

            // Spawn weapon to hand position
            Weapon newWeapon = Instantiate<WorldItem>(equipmentItem.itemPrefab).GetComponent<Weapon>();
            newWeapon.transform.SetParent(weaponHoldTransform, false);
            newWeapon.transform.localRotation = Quaternion.identity;

            // Assign spawned weapon to weapon slot
            weaponSlots[weaponSlotID] = newWeapon;

            // Update current active weapon if we've changed it
            if(weaponSlotID == activeWeaponSlotID)
            {
                SwapWeapon(weaponSlot);
            }
        }

        /// <summary>
        /// Unequips the weapon associated with the equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the slot to unequip.</param>
        public void UnequipWeaponFromSlot(int equipmentSlotID)
        {
            // if(equipmentSlotID < 0 || equipmentSlotID >= equipmentSlots.Length)
            // {
            //     Debug.LogWarning("Equipment Slot doesn't exist. The weapon wasn't unequipped.");
            //     return;
            // }
            
            // equipmentSlots[equipmentSlotID] = null;

            // if(equipmentSlotID == activeWeaponSlotID)
            // {
            //     SwapWeapon(equipmentSlotID);
            // }
        }

        /// <summary>
        /// Swaps the active equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the slot to make active.</param>
        public void SwapWeapon(WeaponSlot weaponSlot)
        {
            int weaponSlotID = (int) weaponSlot;

            // If we currently have an active weapon...
            if(ActiveWeapon != null)
            {
                // Unequip current weapon
                ActiveWeapon.isEquipped = false;
                ActiveWeapon.SetCanUse(false);

                // TODO: Animate weapon swap with gameobject hide/move callback on complete (possibly WeaponSlot class as helper?)

                // **** TEST SWAP ****
                ActiveWeapon.gameObject.SetActive(false);
            }
            
            // switch the active equipment slot
            activeWeaponSlotID = weaponSlotID;

            // If weapon occupies this slot...
            if(ActiveWeapon != null)
            {
                // Set weapon equiped
                ActiveWeapon.isEquipped = true;
                ActiveWeapon.SetCanUse(true);

                // TODO: Animate weapon swap with gameobject hide/move callback on complete (possibly WeaponSlot class as helper?)
                // TODO: Update animator with appropriate weapon hold pose (including idle if no weapon)
                
                // **** TEST SWAP ****
                ActiveWeapon.gameObject.SetActive(true);
            }
            // ...else ensure default anim + settings are being used
            else
            {
                // ensure default
            }

            // TODO: torso animations may need it's own state flow to ensure correct animations are selected e.g. gun idle, to aiming, to swap, to new gun pose, to disabled torso layer, to enabled torso layer

            // ui temporarily appears when swapping - via event
            OnWeaponSwap?.Invoke(activeWeaponSlotID);
        }

        public void UpdateAnimations()
        {
            // Aiming Animation            
            if(ActiveWeapon && aimTargetConstraint)
            {
                playerAnimator.SetLayerWeight(1, (InputManager.Instance.Aim ? 1.0f : 0.0f));
                aimTargetConstraint.weight = InputManager.Instance.Aim ? 1.0f : 0.0f;
            }
        }

        // TODO:
        // change loadout from inventory
    }
}
