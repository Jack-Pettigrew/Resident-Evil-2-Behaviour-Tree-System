using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public static class RoomPathFinder
    {
        /// <summary>
        /// Returns the calculated route from the Starting Room to the Goal Room using Doors as waypoints (A variation of BFS Search).
        /// </summary>
        /// <param name="startingRoom">The starting Room of the path.</param>
        /// <param name="goalRoom">The goal/destination of the path.</param>
        /// <returns>Array of Doors as waypoints to the Goal Room.</returns>
        public static Door[] FindPathToRoom(Room startingRoom, Room goalRoom)
        {
            Dictionary<Door, int> doorCostDictionary = new Dictionary<Door, int>();

            if(!BFSCalculateDoorCosts(startingRoom, goalRoom, ref doorCostDictionary))
            {
                return null;
            }

            return FindLowestCostPath(startingRoom, goalRoom, ref doorCostDictionary);
        }

        private static bool BFSCalculateDoorCosts(Room startingRoom, Room goalRoom, ref Dictionary<Door, int> doorCostDictionary)
        {
            HashSet<Room> roomsToCheck = new HashSet<Room>(); // to check next iteration
            HashSet<Room> roomsCurrentlyChecking = new HashSet<Room>(); // currently checking this iteration

            int currentDistCost = 0;
            roomsToCheck.Add(goalRoom);
            while (roomsToCheck.Count > 0)
            {
                roomsCurrentlyChecking.UnionWith(roomsToCheck);
                roomsToCheck.Clear();

                foreach (var room in roomsCurrentlyChecking)
                {
                    foreach (var door in room.Doors)
                    {
                        if (doorCostDictionary.ContainsKey(door))
                        {
                            continue;
                        }

                        doorCostDictionary.Add(door, currentDistCost);

                        Room linkingRoom = door.RoomA != room ? door.RoomA : door.RoomB;

                        if (linkingRoom == startingRoom)
                        {
                            return true;
                        }
                        else
                        {
                            roomsToCheck.Add(linkingRoom);
                        }
                    }
                }

                roomsCurrentlyChecking.Clear();
                currentDistCost++;
            }

            return false;
        }

        private static Door[] FindLowestCostPath(Room startingRoom, Room goalRoom, ref Dictionary<Door, int> doorCostDictionary)
        {
            List<Door> doorPath = new List<Door>();
            Room currentRoom = startingRoom;
            bool done = false;

            while (!done)
            {
                int cost = int.MaxValue;
                Door cheapestDoor = null;

                foreach (var door in currentRoom.Doors)
                {
                    if (doorCostDictionary[door] < cost)
                    {
                        cost = doorCostDictionary[door];
                        cheapestDoor = door;
                    }
                }

                doorPath.Add(cheapestDoor);

                currentRoom = cheapestDoor.RoomA != currentRoom ? cheapestDoor.RoomA : cheapestDoor.RoomB;
                done = currentRoom == goalRoom;
            }

            return doorPath.ToArray();
        }
    }
}