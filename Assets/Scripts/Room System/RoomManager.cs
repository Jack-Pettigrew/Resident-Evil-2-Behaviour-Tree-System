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
        /// <param name="objectPosition">The position of the gameobject.</param>
        /// <returns>The current Room of the object or null if no room is found.</returns>
        public static Room GetRoomOfObject(Vector3 objectPosition)
        {
            RaycastHit hit;
            Physics.Raycast(objectPosition, Vector3.down, out hit, 10.0f, LayerMask.GetMask("Room"), QueryTriggerInteraction.Ignore);
            Debug.DrawRay(objectPosition, Vector3.down * 10.0f, Color.red, 20.0f, false);

            RoomFloor floor = hit.collider.GetComponent<RoomFloor>();

            return floor ? floor.OwnerRoom : null;
        }

        /// <summary>
        /// Returns whether the given GameObject is inside the given Room.
        /// </summary>
        /// <param name="gameObject">GameObject in question.</param>
        /// <param name="targetRoom">The target Room.</param>
        /// <returns>Is inside the Room?</returns>
        public static bool IsObjectInRoom(GameObject gameObject, Room targetRoom)
        {
            return GetRoomOfObject(gameObject.transform.position) == targetRoom ? true : false;
        }

    }
}