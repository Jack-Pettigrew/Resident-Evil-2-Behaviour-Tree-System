using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Combat;

namespace DD.Core.Control
{
    public class InputManager : MonoBehaviour
    {
        // Singleton
        public static InputManager Instance;

        [Header("Management")]
        [SerializeField] private bool ignoreInput = false;
        public Vector3 InputDirection { private set; get; }

        [Header("Keys")]
        public KeyCode pauseKey;
        public event Action OnPause;

        public KeyCode sprintKeyCode;
        public bool Sprint { private set; get; }

        public KeyCode aimKeyCode;
        public bool Aim { private set; get; }
        public Action OnAimDown;
        public Action OnAimUp;

        public KeyCode shootKeyCode;
        public event Action OnShoot;

        public KeyCode reloadKeyCode;
        public event Action OnReload;

        public KeyCode inventoryKeyCode;
        public event Action OnInventoryToggle;

        public KeyCode quickSlotOneKey, quickSlotTwoKey, quickSlotThreeKey, quickSlotFourKey;
        public event Action<WeaponSlot> OnQuickSlotChange;

        private void Awake()
        {
            Instance = this;
        }

#if UNITY_EDITOR
        private void Start()
        {
            OnPause += CursorToggle;
            OnInventoryToggle += CursorToggle;
        }
#endif

        private void Update()
        {
            if (Input.GetKeyDown(pauseKey)) OnPause?.Invoke();


            // Locomotion
            InputDirection = ignoreInput ? Vector3.zero : new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            
            if (Sprint && InputDirection.sqrMagnitude <= 0 || Aim)
            {
                Sprint = false;
            }
            else if (!Aim && InputDirection.sqrMagnitude > 0 && !Sprint)
            {
                Sprint = Input.GetKeyDown(sprintKeyCode);
            }

            // Combat
            Aim = Input.GetKey(aimKeyCode);

            // Global
            if (ignoreInput) return;
            
            if (Input.GetKeyDown(aimKeyCode)) OnAimDown?.Invoke();
            if (Input.GetKeyUp(aimKeyCode)) OnAimUp?.Invoke();
            if (Input.GetKeyDown(shootKeyCode)) OnShoot?.Invoke();
            if (Input.GetKeyDown(reloadKeyCode)) OnReload?.Invoke();

            // Inventory
            if (Input.GetKeyDown(inventoryKeyCode)) OnInventoryToggle?.Invoke();

            // Quick Slots
            if (Input.GetKeyDown(quickSlotOneKey)) OnQuickSlotChange?.Invoke(WeaponSlot.One);
            if (Input.GetKeyDown(quickSlotTwoKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Two);
            if (Input.GetKeyDown(quickSlotThreeKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Three);
            if (Input.GetKeyDown(quickSlotFourKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Four);
        }

        public void ToggleIgnoreInput(bool toggle) => ignoreInput = toggle;

        public void CursorToggle()
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
