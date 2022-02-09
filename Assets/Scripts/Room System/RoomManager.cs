using System;
using System.Collections;
using UnityEngine;

namespace DD.Systems.Room
{
    public class RoomManager : MonoBehaviour
    {
        private static Room[] rooms;

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