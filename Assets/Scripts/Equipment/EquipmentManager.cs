using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.Core.Items;
using UnityEngine.Animations.Rigging;
using DD.UI;

namespace DD.Core.Combat
{
    public class EquipmentManager : MonoBehaviour
    {
        // Singleton
        public static EquipmentManager Instance { private set; get; }

        // Equipment Slots
        private Weapon[] weaponSlots = new Weapon[4];
        public Weapon[] WeaponSlots { get { return weaponSlots; } }
        private int activeWeaponSlotID = 0;
        public Weapon ActiveWeapon { get { return weaponSlots[activeWeaponSlotID]; } }

        [Header("Animation")]
        [SerializeField] private Transform weaponHoldTransform;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private MultiAimConstraint aimTargetConstraint;

        [Header("UI")]
        [SerializeField] private WeaponSlotPickerUI weaponSlotPickerUI;

        // Events
        public event Action OnWeaponSwap;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            // Input Swap
            InputManager.Instance.OnQuickSlotChange += SwapWeapon;
            InputManager.Instance.OnShoot += UseWeapon;
            InputManager.Instance.OnReload += UseWeaponAction;
        }

        private void LateUpdate() {
            UpdateAnimations();
        }     

        public void UseWeapon()
        {
            ActiveWeapon.Attack();
        }
        
        public void UseWeaponAction()
        {
            ActiveWeapon?.UseWeaponAction();
        }

        public bool HasEquipmentEquipped(EquipmentItem equipmentItem)
        {
            foreach (Weapon weapon in weaponSlots)
            {
                if(weapon != null && weapon.WorldItem.Item == equipmentItem)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Equips the given weapon in the associated equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the equipment slot to set the weapon to.</param>
        /// <param name="weaponToEquip">Weapon to equip.</param>
        public void EquipWeapon(EquipmentItem equipmentItem, WeaponSlot weaponSlot)
        {
            int weaponSlotID = (int) weaponSlot;

            // Handle moving weapon slots
            if(HasEquipmentEquipped(equipmentItem))
            {
                MoveEquipmentWeaponSlot(equipmentItem, weaponSlot);
            }
            else
            {
                // Remove previous Weapon
                GameObject previousWeapon = weaponSlots[weaponSlotID]?.GetComponent<GameObject>();
                if(!previousWeapon)
                {
                    Destroy(previousWeapon);
                }

                // Spawn weapon to hand position + disable
                Weapon newWeapon = Instantiate<WorldItem>(equipmentItem.itemPrefab).GetComponent<Weapon>();
                newWeapon.transform.SetParent(weaponHoldTransform, false);
                newWeapon.transform.localRotation = Quaternion.identity;
                newWeapon.gameObject.SetActive(false);

                // Assign spawned weapon to weapon slot
                weaponSlots[weaponSlotID] = newWeapon;
            }

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
        public void UnequipWeaponFromSlot(WeaponSlot weaponSlot)
        {
            int weaponSlotID = (int) weaponSlot;

            Destroy(weaponSlots[weaponSlotID].gameObject);
            weaponSlots[weaponSlotID] = null;

            if(weaponSlotID == activeWeaponSlotID)
            {
                SwapWeapon(weaponSlot);
            }
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
                ActiveWeapon.SetEquipped(false);
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
                ActiveWeapon.SetEquipped(true);
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
            OnWeaponSwap?.Invoke();
        }

        public void MoveEquipmentWeaponSlot(EquipmentItem equipmentItem, WeaponSlot weaponSlot)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if(weaponSlots[i] != null && weaponSlots[i].WorldItem.Item == equipmentItem)
                {
                    weaponSlots[(int)weaponSlot] = weaponSlots[i];
                    weaponSlots[i] = null;
                    return;
                }
            }
        }

        public void OpenWeaponSlotPicker(EquipmentItem weaponItem)
        {
            weaponSlotPickerUI.SelectWeaponForEquip(weaponItem);
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
    }
}
