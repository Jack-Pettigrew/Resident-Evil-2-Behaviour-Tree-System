using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Door[] doors;

        private void OnDrawGizmosSelected()
        {
            foreach (var door in doors)
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