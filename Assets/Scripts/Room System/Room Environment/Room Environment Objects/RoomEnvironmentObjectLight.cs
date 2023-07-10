using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    /// <summary>
    /// A component dedicated to controlling the behaviour of the Light based on the status of the Room it's owned by.
    /// </summary>
    public class RoomEnvironmentObjectLight : RoomEnvironmentObject
    {
        [field: SerializeField] public bool IsPowered { private set; get; } = true;
        [field: SerializeField] public bool IsOn { private set; get; } = true;

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Backspace))
            {
                TogglePower(true);
            }
        }

        /// <summary>
        /// Toggles whether the light is powered.
        /// </summary>
        /// <param name="toggle"></param>
        public void TogglePower(bool toggle)
        {
            if (toggle == IsPowered) return;

            IsPowered = toggle;

            if (IsPowered && IsOn)
            {
                OnObjectActivated?.Invoke();
            }
            else
            {
                OnObjectDeactivated?.Invoke();
            }
        }

        /// <summary>
        /// Toggle whether the Light is turned on.
        /// </summary>
        /// <param name="toggle"></param>
        public void ToggleLightOn(bool toggle)
        {
            if (toggle == IsOn) return;

            IsOn = toggle;

            if (IsOn && IsPowered)
            {
                OnObjectActivated?.Invoke();

            }
            else
            {
                OnObjectDeactivated?.Invoke();
            }
        }

        protected override void HandleObjectActivated(Room room)
        {
            if (IsPowered && IsOn)
            {
                base.HandleObjectActivated(room);
            }

        }

        protected override void HandleObjectDeactivated(Room room)
        {
            base.HandleObjectDeactivated(room);
        }
    }
}
