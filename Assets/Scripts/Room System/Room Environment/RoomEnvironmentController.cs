using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DD.Core.Control;

namespace DD.Systems.Room
{
    [RequireComponent(typeof(Room))]
    /// <summary>
    /// A Component dedicated to updating registered RoomEnvironmentObjects of the status of the Room they're owned by.
    /// </summary>
    public class RoomEnvironmentController : MonoBehaviour
    {
        // PLAYER
        private GameObject playerGameObject;

        // CONTROLLER VARIABLES
        public Room LinkedRoom { private set; get; }
        public bool IsActive { private set; get; } = false;
        [field: SerializeField] public Room[] ConnectedRooms { private set; get; }
        public List<Door> connectedDoorsInUse { private set; get; } = new List<Door>(4);

        // COMPONENTS
        private Dictionary<Room, RoomEnvironmentController> connectedRoomEnvControllers = new Dictionary<Room, RoomEnvironmentController>();

        // EVENTS
        public event Action<Room> OnRoomActivated;
        public event Action<Room> OnRoomDeactivated;

        private void Start()
        {
            LinkedRoom = GetComponent<Room>();

            playerGameObject = FindAnyObjectByType<PlayerController>().gameObject;

            // Setup up connecting rooms
            foreach (Room connectedRoom in ConnectedRooms)
            {
                if (connectedRoom.TryGetComponent<RoomEnvironmentController>(out RoomEnvironmentController envController))
                {
                    connectedRoomEnvControllers.Add(connectedRoom, envController);
                }
            }

            // Ensure all default to deactivated
            ForceDeactivateRoom();

            
            
            
            if (RoomManager.GetRoomOfObject(playerGameObject) == LinkedRoom)
            {
                ActivateRoom();
                ActivateConnectedRooms();
            }
        }

        /// <summary>
        /// Activating Self
        /// </summary>
        public void ActivateRoom()
        {
            if (IsActive) return;

            IsActive = true;
            OnRoomActivated?.Invoke(LinkedRoom);
        }

        public void DeactivateRoom()
        {
            if (!IsActive) return;

            IsActive = false;
            OnRoomDeactivated?.Invoke(LinkedRoom);
        }

        public void ForceDeactivateRoom()
        {
            IsActive = false;
            OnRoomDeactivated?.Invoke(LinkedRoom);
        }

        /// <summary>
        /// Activating connected
        /// </summary>
        private void ActivateConnectedRooms()
        {
            foreach (var controller in connectedRoomEnvControllers)
            {
                controller.Value.ActivateRoom();
            }

        }
    }
}
