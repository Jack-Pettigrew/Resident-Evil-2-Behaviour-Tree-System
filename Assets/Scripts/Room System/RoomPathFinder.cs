using System.Collections;
using System.Collections.Generic;

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
        public static Door[] FindRouteToRoom(Room startingRoom, Room goalRoom)
        {
            // Get all doors in Room to check
            // give door dist cost
            // if room is startingRoom
            // stop
            // else
            // add room to rooms to check next iteration

            bool foundRoom = false;
            Dictionary<Door, int> doorCostDictionary = new Dictionary<Door, int>();
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
                            foundRoom = true;
                            break;
                        }
                        else
                        {
                            roomsToCheck.Add(linkingRoom);
                        }
                    }

                    if (foundRoom)
                        break;
                }

                if (foundRoom)
                    break;

                roomsCurrentlyChecking.Clear();
                currentDistCost++;
            }

            if (!foundRoom)
                return null;

            // Walk back from startingRoom picking only the doors with the lowest cost
            // add each door to list of doors
            // when reached goal room
            // return list

            List<Door> doorPath = new List<Door>();
            Room currentRoom = startingRoom;
            bool done = false;

            while(!done)
            {
                int cost = int.MaxValue;
                Door cheapestDoor = null;

                foreach (var door in currentRoom.Doors)
                {
                    if(doorCostDictionary[door] < cost)
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