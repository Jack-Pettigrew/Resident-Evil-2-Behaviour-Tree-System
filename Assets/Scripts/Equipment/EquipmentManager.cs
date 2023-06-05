using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.Core.Items;
using DD.UI;
using DD.Animation;
using DD.Core.Items;
using DD.Systems.InventorySystem;

namespace DD.Core.Combat
{
    public class EquipmentManager : MonoBehaviour, IAnimatorEvent<PlayerAnimationController>
    {
        // Singleton
        public static EquipmentManager Instance { private set; get; }

        // Equipment Slots
        private Weapon[] weaponSlots = new Weapon[4];
        public Weapon[] WeaponSlots { get { return weaponSlots; } }
        private int activeWeaponSlotID = 0;
        public Weapon ActiveWeapon { get { return weaponSlots[activeWeaponSlotID]; } }
        [SerializeField] private Transform weaponHoldSocket;

        [Header("UI")]
        [SerializeField] private WeaponSlotPickerUI weaponSlotPickerUI;

        // Events
        public event Action OnWeaponSwapping;
        public event Action OnWeaponSwapped;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Input Swap
            InputManager.Instance.OnQuickSlotChange += SwapWeapon;
            InputManager.Instance.OnShoot += UseWeapon;
            InputManager.Instance.OnReload += UseWeaponAction;
            
            // Inventory Events
            Inventory.Instance.OnItemDropped += itemData => {
                if(itemData is EquipmentItem)
                {
                    UnequipWeapon((EquipmentItem) itemData);
                }
            };
        }

        public void UseWeapon()
        {
            ActiveWeapon?.Attack();
        }

        public void UseWeaponAction()
        {
            ActiveWeapon?.UseWeaponAction();
        }

        public bool IsEquipmentEquipped(EquipmentItem equipmentItem)
        {
            foreach (Weapon weapon in weaponSlots)
            {
                if(weapon == null) continue;
                
                if (weapon.TryGetComponent<WorldItem>(out WorldItem worldItem) && worldItem.ItemData == equipmentItem)
                {
                    return true;
                }
                else
                {
                    Debug.LogError($"{weapon.name}: No WorldItem component attached to this Equipment.", weapon);
                }
            }

            return false;
        }

        public bool IsEquipmentEquipped(EquipmentItem equipmentItem, out WeaponSlot weaponSlot)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if(weaponSlots[i] == null) continue;
                
                if (weaponSlots[i].TryGetComponent<WorldItem>(out WorldItem worldItem) && worldItem.ItemData == equipmentItem)
                {
                    weaponSlot = (WeaponSlot)i;
                    return true;
                }
                else
                {
                    Debug.LogError($"{weaponSlots[i].name}: No WorldItem component attached to this Equipment.", weaponSlots[i]);
                }
            }

            weaponSlot = WeaponSlot.One;
            return false;
        }

        /// <summary>
        /// Equips the given weapon in the associated equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the equipment slot to set the weapon to.</param>
        /// <param name="weaponToEquip">Weapon to equip.</param>
        public void EquipWeaponToSlot(EquipmentItem equipmentItem, WeaponSlot weaponSlot)
        {
            int weaponSlotID = (int)weaponSlot;

            // Handle moving weapon slots
            if (IsEquipmentEquipped(equipmentItem))
            {
                MoveEquipmentToSlot(equipmentItem, weaponSlot);
            }
            else
            {
                // Remove previous physical weapon
                if (weaponSlots[weaponSlotID] != null)
                {
                    Destroy(weaponSlots[weaponSlotID].gameObject);
                    weaponSlots[weaponSlotID] = null;
                }

                // Spawn weapon to hand position + disable
                Weapon newWeapon = Instantiate<WorldItem>(equipmentItem.itemPrefab).GetComponent<Weapon>();
                newWeapon.transform.SetParent(weaponHoldSocket, false);
                newWeapon.transform.localRotation = Quaternion.identity;
                newWeapon.gameObject.SetActive(false);

                if (!newWeapon.TryGetComponent<WorldItem>(out WorldItem worldItem))
                {
                    Debug.LogError($"{newWeapon.name}: No WorldItem component attached to this Equipment.", newWeapon);
                }
                worldItem.CanInteract = false;

                // Assign spawned weapon to weapon slot
                weaponSlots[weaponSlotID] = newWeapon;
            }

            // Update current active weapon if we've changed it
            if (weaponSlotID == activeWeaponSlotID)
            {
                SwapWeapon(weaponSlot);
            }
        }

        public void UnequipWeapon(EquipmentItem equipmentItem)
        {
            if(IsEquipmentEquipped(equipmentItem, out WeaponSlot weaponSlot) && weaponSlots[(int) weaponSlot] != null)
            {
                Destroy(weaponSlots[(int) weaponSlot].gameObject);
                weaponSlots[(int) weaponSlot] = null;
            }

            if (((int) weaponSlot) == activeWeaponSlotID)
            {
                SwapWeapon(weaponSlot);
            }
        }

        /// <summary>
        /// Unequips the weapon associated with the equipment slot.
        /// </summary>
        /// <param name="equipmentSlotID">ID of the slot to unequip.</param>
        public void UnequipWeapon(WeaponSlot weaponSlot)
        {
            int weaponSlotID = (int)weaponSlot;

            if (weaponSlots[weaponSlotID] != null)
            {
                Destroy(weaponSlots[weaponSlotID].gameObject);
                weaponSlots[weaponSlotID] = null;
            }

            if (weaponSlotID == activeWeaponSlotID)
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
            OnWeaponSwapping?.Invoke();

            int weaponSlotID = (int)weaponSlot;

            // If we currently have an active weapon...
            if (ActiveWeapon != null)
            {
                // Unequip current weapon
                ActiveWeapon.SetCanUse(false);

                // TODO: Animate weapon swap with gameobject hide/move callback on complete (possibly WeaponSlot class as helper?)

                // **** TEST SWAP ****
                ActiveWeapon.gameObject.SetActive(false);
            }

            // switch the active equipment slot
            activeWeaponSlotID = weaponSlotID;

            // If weapon occupies this slot...
            if (ActiveWeapon != null)
            {
                // Set weapon equiped
                ActiveWeapon.SetCanUse(true);

                // TODO: Animate weapon swap with gameobject hide/move callback on complete (possibly WeaponSlot class as helper?)
                // TODO: Update animator with appropriate weapon hold pose (including idle if no weapon)

                // **** TEST SWAP ****
                ActiveWeapon.gameObject.SetActive(true);
            }
            // ...else ensure default idle anim + settings are being used
            else
            {
                // ensure default
            }

            // TODO: torso animations may need it's own state flow to ensure correct animations are selected e.g. gun idle, to aiming, to swap, to new gun pose, to disabled torso layer, to enabled torso layer

            OnWeaponSwapped?.Invoke();
        }

        /// <summary>
        /// Moves the given equipment item to another slot.
        /// </summary>
        /// <param name="equipmentItem"></param>
        /// <param name="weaponSlot"></param>
        public void MoveEquipmentToSlot(EquipmentItem equipmentItem, WeaponSlot weaponSlot)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i] != null && weaponSlots[i].TryGetComponent<WorldItem>(out WorldItem worldItem))
                {
                    if (worldItem.ItemData == equipmentItem)
                    {
                        weaponSlots[(int)weaponSlot] = weaponSlots[i];
                        weaponSlots[i] = null;
                        return;
                    }
                }
                else {
                    Debug.LogError($"{weaponSlots[i].name}: No WorldItem component attached to this Equipment.", weaponSlots[i]);
                }
            }
        }

        public void SubscribeAnimator(PlayerAnimationController animationController)
        {
            OnWeaponSwapped += animationController.UpdateWeaponType;
        }

        public void UnsubscribeAnimator(PlayerAnimationController animationController)
        {
            OnWeaponSwapped -= animationController.UpdateWeaponType;
        }
    }
}
