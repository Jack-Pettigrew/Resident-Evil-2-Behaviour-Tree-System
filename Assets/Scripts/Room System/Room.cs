using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class Room : MonoBehaviour
    {
        [SerializeField, Tooltip("Points defining where NPCs should consider 'searching'.")] private Transform[] searchSpots;
        public Transform[] SearchSpots { set { searchSpots = value; } get { return searchSpots; } }

        [SerializeField] private Door[] doors;
        public Door[] Doors { set { doors = value; } get { return doors; } }

        private void Awake()
        {
            foreach (var door in Doors)
            {
                if (door == null)
                {
                    Debug.LogError($"{this} has a NULL Door in it's array - Doors array was reassigned to empty array.");
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
                    if (door.RoomA && door.RoomA != this)
                        Gizmos.DrawLine(door.transform.position, door.RoomA.transform.position);
                    if (door.RoomB && door.RoomB != this)
                        Gizmos.DrawLine(door.transform.position, door.RoomB.transform.position);
                }
            }
        }
    }
}