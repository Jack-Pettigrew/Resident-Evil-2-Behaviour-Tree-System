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
    }
}