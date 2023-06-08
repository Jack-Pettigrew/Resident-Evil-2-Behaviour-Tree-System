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
        private Room linkedRoom;

        [SerializeField] private Room[] connectedRooms;
        // [SerializeField] private Room[] roomsToIgnore;

        private Dictionary<Room, RoomEnvironmentController> roomEnvironmentControllerDictionary = new Dictionary<Room, RoomEnvironmentController>();

        private static GameObject playerGameObject;

        public UnityEvent OnRoomActivated;
        public UnityEvent OnRoomDeactivated;

        private void Awake() {
            linkedRoom = GetComponent<Room>();
            
            if(!playerGameObject)
            {
                playerGameObject = FindAnyObjectByType<PlayerController>().gameObject;
            }

            foreach (Room connectedRoom in connectedRooms)
            {
                if(connectedRoom.TryGetComponent<RoomEnvironmentController>(out RoomEnvironmentController envController))
                {
                    roomEnvironmentControllerDictionary.Add(connectedRoom, envController);
                }
            }

            OnRoomDeactivated?.Invoke();
            if(RoomManager.GetRoomOfObject(playerGameObject) == linkedRoom)
            {
                OnRoomActivated?.Invoke();
                
                foreach (Room room in connectedRooms)
                {
                    roomEnvironmentControllerDictionary[room].OnRoomActivated?.Invoke();
                }
            }
        }

        private void OnEnable()
        {
            if (!linkedRoom) Debug.LogError("Linked Room is required for this RoomEnvironmentController", this);

            foreach (Door door in linkedRoom.Doors)
            {
                door.openingDoorEvent.AddListener(HandleRoomActive);
                door.closedDoorEvent.AddListener(HandleRoomDeactivated);
            }
        }

        private void OnDisable()
        {
            foreach (Door door in linkedRoom.Doors)
            {
                door.openingDoorEvent.RemoveListener(HandleRoomActive);
                door.closedDoorEvent.RemoveListener(HandleRoomDeactivated);
            }
        }

        public void HandleRoomActive(Door connectingRoomDoor)
        {
            if(!IsRoomIgnored(connectingRoomDoor))
            {
                OnRoomActivated?.Invoke();

                // Handle Connecting Rooms
                if(roomEnvironmentControllerDictionary.ContainsKey(connectingRoomDoor.RoomA))
                {
                    roomEnvironmentControllerDictionary[connectingRoomDoor.RoomA].OnRoomActivated?.Invoke();
                }

                if(roomEnvironmentControllerDictionary.ContainsKey(connectingRoomDoor.RoomB))
                {
                    roomEnvironmentControllerDictionary[connectingRoomDoor.RoomB].OnRoomActivated?.Invoke();
                }
            }
        }

        public void HandleRoomDeactivated(Door connectingRoomDoor)
        {
            if(!IsRoomIgnored(connectingRoomDoor) && RoomManager.GetRoomOfObject(playerGameObject) != linkedRoom)
            {
                OnRoomDeactivated?.Invoke();

                // Handle Connecting Rooms
                if(roomEnvironmentControllerDictionary.ContainsKey(connectingRoomDoor.RoomA))
                {
                    roomEnvironmentControllerDictionary[connectingRoomDoor.RoomA].OnRoomDeactivated?.Invoke();
                }

                if(roomEnvironmentControllerDictionary.ContainsKey(connectingRoomDoor.RoomB))
                {
                    roomEnvironmentControllerDictionary[connectingRoomDoor.RoomB].OnRoomDeactivated?.Invoke();
                }
            }
        }

        private bool IsRoomIgnored(Door connectingRoomDoor)
        {
            foreach (Room ignoreRoom in connectedRooms)
            {
                if(connectingRoomDoor.RoomA == ignoreRoom || connectingRoomDoor.RoomB == ignoreRoom)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
