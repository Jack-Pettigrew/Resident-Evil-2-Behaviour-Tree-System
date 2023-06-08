using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Systems.Room
{
    /// <summary>
    /// A component dedicated to controlling the behaviour of the GameObject based on the status of the Room it's owned by.
    /// </summary>
    public class RoomEnvironmentObject : MonoBehaviour
    {
        public RoomEnvironmentController roomEnvironmentController;
        
        // UNITY EVENTS - in addition to RoomEnvironmentController for editor serialisation
        public UnityEvent OnRoomActivated;
        public UnityEvent OnRoomDeactivated;

        private void OnEnable() {
            if(!roomEnvironmentController) return;
            
            roomEnvironmentController.OnRoomActivated.AddListener(OnRoomActivated.Invoke);
            roomEnvironmentController.OnRoomDeactivated.AddListener(OnRoomDeactivated.Invoke);
        }

        private void OnDisable() {
            if(!roomEnvironmentController) return;
            
            roomEnvironmentController.OnRoomActivated.RemoveListener(OnRoomActivated.Invoke);
            roomEnvironmentController.OnRoomDeactivated.RemoveListener(OnRoomDeactivated.Invoke);
        }
    }
}
