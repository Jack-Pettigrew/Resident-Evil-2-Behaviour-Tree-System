using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class RoomManager : MonoBehaviour
    {
        private static Room[] rooms;

        private static Dictionary<Room, Room[]> roomConnections = new Dictionary<Room, Room[]>();

        private void Awake()
        {
            GatherAllRooms();
        }

        /// <summary>
        /// Gets and stores all the currently loaded Rooms in the scene.
        /// </summary>
        public static void GatherAllRooms()
        {
            rooms = FindObjectsOfType<Room>();

            foreach (Room room in rooms)
            {
                Room[] connectingRooms = new Room[room.Doors.Length];
                
                for (int i = 0; i < room.Doors.Length; i++)
                {
                    connectingRooms[i] = room.Doors[i].RoomA != room ? room.Doors[i].RoomA : room.Doors[i].RoomB;
                }

                roomConnections.Add(room, connectingRooms);
            }
        }

        /// <summary>
        /// Returns a random Room from those currently gathered.
        /// </summary>
        /// <returns></returns>
        public static Room GetRandomRoom()
        {
            return rooms[UnityEngine.Random.Range(0, rooms.Length)];
        }

        /// <summary>
        /// Returns a random room adjacent to the given room, or the given room if desired.
        /// </summary>
        /// <param name="roomToBaseOn">The Room to randomly select the adjacent rooms of.</param>
        /// <param name="includeBaseRoom">Should the given room included in the random selection?</param>
        /// <returns>A randomly selected room.</returns>
        public static Room GetRandomAdjacentRoom(Room roomToBaseOn, bool includeBaseRoom)
        {
            Room[] connectingRooms;
            roomConnections.TryGetValue(roomToBaseOn, out connectingRooms);

            if(connectingRooms == null) return null;

            if(includeBaseRoom)
            {
                // Get random index including out of bounds (make sure it's not NULL)
                int randomIndex = 0;
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, connectingRooms.Length + 1);
                }
                while(randomIndex < connectingRooms.Length && connectingRooms[randomIndex] == null);

                // If out of bounds, return the room random is based on
                return randomIndex == connectingRooms.Length ? roomToBaseOn : connectingRooms[randomIndex];
            }
            else
            {
                // Get random index including out of bounds (make sure it's not NULL)
                int randomIndex = 0;
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, connectingRooms.Length);
                }
                while(connectingRooms[randomIndex] == null);

                return connectingRooms[randomIndex];
            }
        }

        /// <summary>
        /// Returns the Room the given position is on.
        /// </summary>
        /// <param name="gameObject">The gameobject.</param>
        /// <returns>The current Room of the object or null if no room is found.</returns>
        public static Room GetRoomOfObject(GameObject gameObject)
        {
            RaycastHit hit;

            // Raycast accounting for GameObject center point margin of error (e.g. when the center point is directly against a floor plane)
            if (Physics.Raycast(gameObject.transform.position + Vector3.up * 0.05f, Vector3.down, out hit, 5.0f, LayerMask.GetMask("Room"), QueryTriggerInteraction.Ignore))
            {
                RoomFloor floor = hit.collider.GetComponent<RoomFloor>();
                return floor ? floor.OwnerRoom : null;
            }

            return null;
        }

        /// <summary>
        /// Returns whether the given GameObject is inside the given Room.
        /// </summary>
        /// <param name="gameObject">GameObject in question.</param>
        /// <param name="targetRoom">The target Room.</param>
        /// <returns>Is inside the Room?</returns>
        public static bool IsObjectInRoom(GameObject gameObject, Room targetRoom)
        {
            if(!gameObject || !targetRoom)
            {
                return false;
            }

            return GetRoomOfObject(gameObject) == targetRoom ? true : false;
        }

    }
}