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
        [field: SerializeField] private PowerSource powerSource;
        [field: SerializeField] public bool IsOn { private set; get; } = true;

        private void Awake() {
            if(powerSource)
            {
                powerSource.OnPoweredOn += () => ToggleLightOn(true);
                powerSource.OnPoweredOff += () => ToggleLightOn(false);

                ToggleLightOn(powerSource.IsPoweredOn);
            }
            else
            {
                ToggleLightOn(false);
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

            if (IsOn && powerSource.IsPoweredOn)
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
            if (powerSource && powerSource.IsPoweredOn && IsOn)
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
