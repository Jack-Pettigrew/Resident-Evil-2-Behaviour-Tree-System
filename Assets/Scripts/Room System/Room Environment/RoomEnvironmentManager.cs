using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    /// <summary>
    /// The overarching manager class for RoomEnvironmentControllers.
    /// </summary>
    public class RoomEnvironmentManager : MonoBehaviour
    {
        public GameObject player;

        private Door[] doors;
        private List<Door> activeDoors = new List<Door>();
        private Dictionary<Room, RoomEnvironmentController> roomEnvironmentControllers = new Dictionary<Room, RoomEnvironmentController>();
        private List<RoomEnvironmentController> activatedRoomControllers = new List<RoomEnvironmentController>();

        // Component
        private PowerSource powerSource;

        private void Awake()
        {
            doors = FindObjectsOfType<Door>();

            foreach (Room room in FindObjectsOfType<Room>())
            {
                if (room.TryGetComponent<RoomEnvironmentController>(out RoomEnvironmentController controller))
                {
                    roomEnvironmentControllers.Add(room, controller);
                }
            }

            powerSource = FindObjectOfType<PowerSource>();
        }

        private void OnEnable()
        {
            powerSource.OnPoweredOn += HandlePowerOn;
            powerSource.OnPoweredOff += HandlePowerOff;
            
            foreach (Door door in doors)
            {
                door.openingDoorEvent.AddListener(HandleDoorOpening);
                door.closedDoorEvent.AddListener(HandleDoorClosed);
            }
        }

        private void OnDisable()
        {
            powerSource.OnPoweredOn -= HandlePowerOn;
            powerSource.OnPoweredOff -= HandlePowerOff;
            
            foreach (Door door in doors)
            {
                door.openingDoorEvent.RemoveListener(HandleDoorOpening);
                door.closedDoorEvent.RemoveListener(HandleDoorClosed);
            }
        }

        private void HandlePowerOn()
        {
            // Activate Player's current Room
            if(RoomManager.GetRoomOfObject(player).TryGetComponent(out Room playerRoom))
            {
                roomEnvironmentControllers[playerRoom].ForceActivateRoom();
            }

            // Activate any active
            foreach (RoomEnvironmentController roomEnvironmentController in activatedRoomControllers)
            {
                roomEnvironmentController.ForceActivateRoom();
            }
        }

        private void HandlePowerOff()
        {
            // Deactivate all active room env controllers
            foreach ((Room room, RoomEnvironmentController roomEnvironmentController) in roomEnvironmentControllers)
            {
                roomEnvironmentController.DeactivateRoom();
            }

            activatedRoomControllers.Clear();
        }

        private void HandleDoorOpening(Door openingDoor)
        {            
            if(!activeDoors.Contains(openingDoor))
            {
                activeDoors.Add(openingDoor);
            }
            
            // Activate both rooms
            if(openingDoor.RoomA)
            {
                ActivateRoom(openingDoor.RoomA);
            }

            if(openingDoor.RoomB)
            {
                ActivateRoom(openingDoor.RoomB);
            }
        }

        private void ActivateRoom(Room room)
        {
            if (roomEnvironmentControllers.TryGetValue(room, out RoomEnvironmentController roomAController))
            {
                // Activate base Room
                roomAController.ActivateRoom();
                LogActiveRoom(roomAController);

                // Activate any Rooms connected to the base Room
                foreach (Room connectedRoom in roomAController.ConnectedRooms)
                {
                    roomEnvironmentControllers[connectedRoom].ActivateRoom();
                    LogActiveRoom(roomEnvironmentControllers[connectedRoom]);
                }
            }
        }

        private void LogActiveRoom(RoomEnvironmentController controller)
        {
            if (activatedRoomControllers.Contains(controller)) return;

            activatedRoomControllers.Add(controller);
        }

        private void HandleDoorClosed(Door closedDoor)
        {
            // Deregister active Doors
            if(activeDoors.Contains(closedDoor))
            {
                activeDoors.Remove(closedDoor);
            }

            // Guard to ensure logic only occurs when no doors are active
            if(activeDoors.Count > 0) return;

            // Get the player's current room
            Room playersRoom = RoomManager.GetRoomOfObject(player);

            // Maintain player's room and it's connected rooms
            ActivateRoom(playersRoom);
            
            // Extract left over active rooms not related to player's current room and it's connected rooms
            // Using LINQ here which isn't ideal but annoyingly it works and reduces a lot code in exchange for performance overhead
            RoomEnvironmentController[] controllers = activatedRoomControllers.Where(controller => !roomEnvironmentControllers[playersRoom].ConnectedRooms.Any(room => controller.LinkedRoom == room))
                .Where(controller => controller.LinkedRoom != playersRoom)
                .ToArray();

            // Clean up and deactivate left over active Rooms
            foreach (RoomEnvironmentController controller in controllers)
            {
                controller.DeactivateRoom();
                activatedRoomControllers.Remove(controller);
            }
        }
    }
}