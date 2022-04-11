using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Control
{
    public class InputManager : MonoBehaviour
    {
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

        private void Update()
        {
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
        }

        public void ToggleIgnoreInput(bool toggle) => ignoreInput = toggle;
    }
}
