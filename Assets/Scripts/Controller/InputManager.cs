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
        private static InputManager instance;
        public static InputManager Instance
        {
            get {
                if(instance == null) {
                    instance = FindObjectOfType<InputManager>();
                }

                return instance;
            }
        }

        [Header("Management")]
        [SerializeField] private bool ignoreInput = false;
        [SerializeField] private bool ignoreMovement = false;
        public Vector3 InputDirection { private set; get; }

        [Header("Camera")]
        public Vector2 pitchYaw = Vector2.zero;

        [Header("Keys")]
        public KeyCode pauseKey;

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
        public event Action OnInventoryPressed;

        public KeyCode quickSlotOneKey, quickSlotTwoKey, quickSlotThreeKey, quickSlotFourKey;
        public event Action<WeaponSlot> OnQuickSlotChange;

        private void OnEnable() {
            GameManager.OnGamePause += CursorToggle;
            GameManager.OnGamePause += ToggleIgnoreInput;
        }

        private void OnDisable() {
            GameManager.OnGamePause -= CursorToggle;
            GameManager.OnGamePause -= ToggleIgnoreInput;
        }

        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                if(GameManager.GameState == GameState.PLAYING)
                {
                    GameManager.PauseGame();
                }
                else
                {
                    GameManager.UnpauseGame();
                }
            } 

            pitchYaw = ignoreInput || ignoreMovement ? Vector2.zero : new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            
            // Locomotion
            InputDirection = ignoreInput || ignoreMovement ? Vector3.zero : new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            
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
            if (Input.GetKeyDown(inventoryKeyCode)) OnInventoryPressed?.Invoke();

            // Quick Slots
            if (Input.GetKeyDown(quickSlotOneKey)) OnQuickSlotChange?.Invoke(WeaponSlot.One);
            if (Input.GetKeyDown(quickSlotTwoKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Two);
            if (Input.GetKeyDown(quickSlotThreeKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Three);
            if (Input.GetKeyDown(quickSlotFourKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Four);
        }

        public void ToggleIgnoreMovement(bool toggle) => ignoreMovement = toggle;

        public void ToggleIgnoreInput(bool toggle) => ignoreInput = toggle;

        public void CursorToggle(bool toggle)
        {
            if(GameManager.GameState == GameState.MAIN_MENU) return;
            
            Cursor.visible = toggle;
            Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
