using DD.AI;
using DD.AI.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class Door : MonoBehaviour
    {
        // STATE
        public bool IsOpen { private set; get; }

        // CONNECTING ROOMS
        [SerializeField] private Room roomA, roomB;
        public Room RoomA { get { return roomA; } }
        public Room RoomB { get { return roomB; } }

        public bool OpenDoor()
        {
            // Set door to open
            // Set IsOpen true
            return true;
        }

        public bool CloseDoor()
        {
            // Set door to close
            // Set IsOpen false
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            if(RoomA)
            {
                Gizmos.DrawLine(transform.position, RoomA.transform.position);
            }

            if(RoomB)
            {
                Gizmos.DrawLine(transform.position, RoomB.transform.position);
            }
        }
    }
}