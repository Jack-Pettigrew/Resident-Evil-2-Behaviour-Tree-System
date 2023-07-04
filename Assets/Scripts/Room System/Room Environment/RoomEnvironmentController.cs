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
        private Room linkedRoom;
        public bool IsActive { private set; get; } = false;
        [SerializeField] private Room[] connectedRooms;
        public List<Door> connectedDoorsInUse {private set; get; } = new List<Door>(4);

        // COMPONENTS
        private Dictionary<Room, RoomEnvironmentController> connectedRoomEnvControllers = new Dictionary<Room, RoomEnvironmentController>();

        // EVENTS
        public event Action<Room> OnRoomActivated;
        public event Action<Room> OnRoomDeactivated;

        private void Awake()
        {
            linkedRoom = GetComponent<Room>();

            playerGameObject = FindAnyObjectByType<PlayerController>().gameObject;

            // Setup up connecting rooms
            foreach (Room connectedRoom in connectedRooms)
            {
                if (connectedRoom.TryGetComponent<RoomEnvironmentController>(out RoomEnvironmentController envController))
                {
                    connectedRoomEnvControllers.Add(connectedRoom, envController);
                }
            }
        }

        private void OnEnable()
        {
            if (!linkedRoom) Debug.LogError("Linked Room is required for this RoomEnvironmentController", this);

            foreach (Door door in linkedRoom.Doors)
            {
                door.openingDoorEvent.AddListener(HandleDoorOpened);
                door.closedDoorEvent.AddListener(HandleDoorClosed);
            }
        }

        private void OnDisable()
        {
            foreach (Door door in linkedRoom.Doors)
            {
                door.openingDoorEvent.RemoveListener(HandleDoorOpened);
                door.closedDoorEvent.RemoveListener(HandleDoorClosed);
            }
        }

        private void Start()
        {
            // Ensure all default to deactivated
            DeactivateRoom();

            if (RoomManager.GetRoomOfObject(playerGameObject) == linkedRoom)
            {
                ActivateRoom();
                ActivateConnectedRooms();
            }
        }

        private void HandleDoorOpened(Door openedDoor)
        {
            if (!connectedDoorsInUse.Contains(openedDoor))
            {
                connectedDoorsInUse.Add(openedDoor);
            }

            ActivateRoom();
            ActivateConnectedRooms();
        }

        private void HandleDoorClosed(Door closedDoor)
        {
            if (connectedDoorsInUse.Contains(closedDoor))
            {
                connectedDoorsInUse.Remove(closedDoor);
            }

            Room playerRoom = RoomManager.GetRoomOfObject(playerGameObject);

            // Using LINQ here which isn't ideal but annoyingly it works and reduces a lot code in exchange for performance
            if (playerRoom == linkedRoom || connectedRooms.Count(room => room == playerRoom) > 0 || connectedDoorsInUse.Count > 0 || connectedRoomEnvControllers.Where((value) => value.Value.connectedDoorsInUse.Count > 0).Count() > 0)
            {
                return;
            }

            DeactivateRoom();
            DeactivateConnectedRooms();
        }

        /// <summary>
        /// Activating Self
        /// </summary>
        private void ActivateRoom()
        {
            IsActive = true;
            OnRoomActivated?.Invoke(linkedRoom);
        }

        private void DeactivateRoom()
        {
            IsActive = false;
            OnRoomDeactivated?.Invoke(linkedRoom);
        }

        /// <summary>
        /// Activating connected
        /// </summary>
        private void ActivateConnectedRooms()
        {
            foreach (var controller in connectedRoomEnvControllers)
            {
                controller.Value.ActivateRoom(); //HandleConnectedActivation(linkedRoom);
            }
        }

        private void DeactivateConnectedRooms()
        {
            foreach (var controller in connectedRoomEnvControllers)
            {
                controller.Value.DeactivateRoom(); // HandleConnectedDeactivation(linkedRoom);
            }
        }
    }
}
