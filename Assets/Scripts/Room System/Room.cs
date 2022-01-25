using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Door[] doors;
        public Door[] Doors { set { doors = value; } get { return doors; } }

        [SerializeField] private RoomFloor[] roomFloors;

        private void Awake()
        {
            if (roomFloors.Length <= 0)
            {
                Debug.LogError(this + " has no child RoomFloor(s). Please assign floors to allow the Room System to know which floors link to which Rooms.");
            }
            else
            {
                foreach (var door in Doors)
                {
                    if(door == null)
                    {
                        Debug.LogError($"{this} has a NULL Door in it's array - Doors array was reassigned to empty array.");
                    }
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// An Editor tool method for setting the this Room's Floor to those selected.
        /// </summary>
        /// <param name="floors"></param>
        public void SetRoomFloors(RoomFloor[] floors)
        {
            foreach (var floor in floors)
            {
                floor.SetOwnerRoom(this);
            }

            roomFloors = floors;
        }
#endif

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