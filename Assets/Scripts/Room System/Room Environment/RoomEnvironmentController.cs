using System;
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
        private Dictionary<Room, RoomEnvironmentController> roomEnvironmentControllerDictionary = new Dictionary<Room, RoomEnvironmentController>();

        private List<Door> connectedDoorsInUse = new List<Door>(4);

        // EVENTS
        public event Action OnRoomActivated;
        public event Action OnRoomDeactivated;

        private void Awake()
        {
            linkedRoom = GetComponent<Room>();

            playerGameObject = FindAnyObjectByType<PlayerController>().gameObject;

            // Setup up connecting rooms
            foreach (Room connectedRoom in connectedRooms)
            {
                if (connectedRoom.TryGetComponent<RoomEnvironmentController>(out RoomEnvironmentController envController))
                {
                    roomEnvironmentControllerDictionary.Add(connectedRoom, envController);
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

        private void Start() {
            // Ensure all default to deactivated
            OnRoomDeactivated?.Invoke();

            if (RoomManager.GetRoomOfObject(playerGameObject) == linkedRoom)
            {
                OnRoomActivated?.Invoke();

                foreach (Room room in connectedRooms)
                {
                    roomEnvironmentControllerDictionary[room].OnRoomActivated?.Invoke();
                }
            }
        }

        private void HandleDoorOpened(Door connectingRoomDoor)
        {
            connectedDoorsInUse.Add(connectingRoomDoor);

            HandleRoomActive();
        }

        private void HandleDoorClosed(Door connectingRoomDoor)
        {
            connectedDoorsInUse.Remove(connectingRoomDoor);

            if(connectedDoorsInUse.Count <= 0)
            {
                HandleRoomDeactivated();
            }
        }

        private void HandleRoomActive()
        {
            if(IsActive) return;
            
            IsActive = true;
            OnRoomActivated?.Invoke();

            // Handle connected rooms
            foreach (Room room in connectedRooms)
            {
                if(roomEnvironmentControllerDictionary.TryGetValue(room, out RoomEnvironmentController controller))
                {
                    controller.HandleRoomActive();
                }
            }
        }

        private void HandleRoomDeactivated()
        {
            if(!IsActive || RoomManager.GetRoomOfObject(playerGameObject) == linkedRoom) return;
            
            IsActive = false;
            OnRoomDeactivated?.Invoke();

            // Handle connected rooms
            foreach (Room room in connectedRooms)
            {
                if(roomEnvironmentControllerDictionary.TryGetValue(room, out RoomEnvironmentController controller))
                {
                    controller.HandleRoomDeactivated();
                }
            }
        }
    }
}
