using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Door[] doors;
        public Door[] Doors { private set { doors = value; } get { return doors; } }

        [SerializeField] private RoomFloor[] roomFloors;


        private void Awake()
        {
            if (roomFloors.Length <= 0)
            {
                Debug.LogError(this + " has no child RoomFloor(s). Please assign floors to allow the Room System to know which floors link to which Rooms.");
            }
            else
            {
                foreach (var floor in roomFloors)
                {
                    floor.SetOwnerRoom(this);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var door in Doors)
            {
                if (door)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(transform.position, door.transform.position);

                    Gizmos.color = Color.green;
                    if(door.RoomA && door.RoomA != this)
                        Gizmos.DrawLine(door.transform.position, door.RoomA.transform.position);
                    if (door.RoomB && door.RoomB != this)
                        Gizmos.DrawLine(door.transform.position, door.RoomB.transform.position);
                }
            }
        }
    }
}