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
        public KeyCode sprintKeyCode;
        public bool Sprint { private set; get; }

        public KeyCode aimKeyCode;
        public bool Aim { private set; get; }

        public KeyCode shootKeyCode;
        public event Action OnShoot;

        public KeyCode reloadKeyCode;
        public event Action OnReload;

        public KeyCode quickSlotOneKey, quickSlotTwoKey, quickSlotThreeKey, quickSlotFourKey;
        public event Action<WeaponSlot> OnQuickSlotChange;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            // Global
            if(ignoreInput) return;

            // Locomotion
            InputDirection =  ignoreInput ? Vector3.zero : new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            if(Sprint && InputDirection.sqrMagnitude <= 0)
            {
                Sprint = false;
            }
            else if(!Aim && !Sprint)
            {
                Sprint = Input.GetKeyDown(sprintKeyCode);
            }

            // Combat
            Aim = Input.GetKey(aimKeyCode);
            if(Input.GetKeyDown(shootKeyCode)) OnShoot?.Invoke();
            if(Input.GetKeyDown(reloadKeyCode)) OnReload?.Invoke();

            // Quick Slots
            if(Input.GetKeyDown(quickSlotOneKey)) OnQuickSlotChange?.Invoke(WeaponSlot.One);
            if(Input.GetKeyDown(quickSlotTwoKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Two);
            if(Input.GetKeyDown(quickSlotThreeKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Three);
            if(Input.GetKeyDown(quickSlotFourKey)) OnQuickSlotChange?.Invoke(WeaponSlot.Four);
        }

        public void ToggleIgnoreInput(bool toggle) => ignoreInput = toggle;
    }
}
