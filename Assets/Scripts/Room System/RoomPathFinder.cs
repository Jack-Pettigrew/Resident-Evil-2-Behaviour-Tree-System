using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public static class RoomPathFinder
    {
        private static Dictionary<Door, int> doorCostDictionary = new Dictionary<Door, int>();

        private static HashSet<Room> roomsToCheck = new HashSet<Room>(); // to check next iteration
        private static HashSet<Room> roomsCurrentlyChecking = new HashSet<Room>(); // currently checking this iteration

        /// <summary>
        /// Returns the calculated route from the Starting Room to the Goal Room using Doors as waypoints (A variation of BFS Search).
        /// </summary>
        /// <param name="startingRoom">The starting Room of the path.</param>
        /// <param name="goalRoom">The goal/destination of the path.</param>
        /// <param name="accountForCanUse">Whether to account for if the Door can be used.</param>
        /// <returns>Array of Doors as waypoints to the Goal Room or null if no path was found.</returns>
        public static Door[] FindDoorPathToRoom(Room startingRoom, Room goalRoom, bool accountForCanUse = true)
        {
            Reset();

            if(!BFSCalculateDoorCosts(startingRoom, goalRoom, accountForCanUse))
            {
                //throw new System.Exception("RoomPathFinder: BFS Path unable to calculate.");
                return null;
            }

            return FindLowestCostPath(startingRoom, goalRoom);
        }

        private static bool BFSCalculateDoorCosts(Room startingRoom, Room goalRoom, bool accountForCanUse = true)
        {
            int currentDistCost = 0;
            roomsToCheck.Add(goalRoom);
            while (roomsToCheck.Count > 0)
            {
                roomsCurrentlyChecking.UnionWith(roomsToCheck);
                roomsToCheck.Clear();

                foreach (Room room in roomsCurrentlyChecking)
                {
                    if(!room || room.Doors == null || room.Doors.Length == 0) continue;
                                        
                    foreach (Door door in room.Doors)
                    {                                        
                        // Account for Door's CanAIUse
                        if(accountForCanUse && !door.CanAIUse) continue;
                        
                        // Is Door locked OR already accounting for Door?
                        if (door.IsLocked || doorCostDictionary.ContainsKey(door))
                        {
                            continue;
                        }

                        doorCostDictionary.Add(door, currentDistCost);

                        // Get the next Room to check
                        Room linkingRoom = door.RoomA != room ? door.RoomA : door.RoomB;

                        // Found starting Room? Finish
                        if (linkingRoom == startingRoom)
                        {
                            return true;
                        }
                        // Add room to check list
                        else if (linkingRoom)
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

        private static Door[] FindLowestCostPath(Room startingRoom, Room goalRoom)
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
                    if (doorCostDictionary.ContainsKey(door) && doorCostDictionary[door] < cost)
                    {
                        cost = doorCostDictionary[door];
                        cheapestDoor = door;
                    }
                }

                // Throw exception on duplicate door
                if(doorPath.Contains(cheapestDoor))
                {
                    throw new System.Exception("Duplicate door while calculating door path!");
                }

                doorPath.Add(cheapestDoor);

                currentRoom = cheapestDoor.RoomA != currentRoom ? cheapestDoor.RoomA : cheapestDoor.RoomB;
                done = currentRoom == goalRoom;
            }

            return doorPath.ToArray();
        }
        private static void Reset()
        {
            doorCostDictionary.Clear();
            roomsToCheck.Clear();
            roomsCurrentlyChecking.Clear();
        }

        /// <summary>
        /// Determines whether a path to the provided Room exists.
        /// </summary>
        /// <param name="startingRoom">The starting Room of the path.</param>
        /// <param name="goalRoom">The goal/destination of the path.</param>
        /// <param name="accountForCanUse">Whether to account for if the Door can be used.</param>
        /// <returns></returns>
        public static bool DoesPathToRoomExist(Room startingRoom, Room goalRoom, bool accountForCanUse = true)
        {
            Reset();
            bool result = BFSCalculateDoorCosts(startingRoom, goalRoom, accountForCanUse);
            return result;
        }
    }
}