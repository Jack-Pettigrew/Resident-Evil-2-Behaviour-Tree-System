using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

public class RoomPathTest : MonoBehaviour
{
    public Room startRoom;
    public Room goalRoom;
    public List<Door> doors;

    private void Start()
    {
        doors.AddRange(RoomPathFinder.FindDoorPathToRoom(startRoom, goalRoom));
    }
}
