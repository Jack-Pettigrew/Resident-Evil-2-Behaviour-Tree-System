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

        [Header("Connecting Room")]
        [SerializeField] private Room roomA;
        public Transform roomAEntryPoint;

        [Header("Connecting Room")]
        [SerializeField] private Room roomB;
        public Transform roomBEntryPoint;

        public Room RoomA { get { return roomA; } }
        public Room RoomB { get { return roomB; } }


        private void Awake()
        {
            if(RoomA == null || RoomB == null)
            {
                //Debug.LogError($"{gameObject} needs entry point Transforms assigning!");
            }
        }

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

        /// <summary>
        /// Returns this Door's entry point that is closest to the given position.
        /// </summary>
        /// <param name="objectsWorldPosition">The position of the object in world space.</param>
        /// <returns>The closest entry point transform.</returns>
        public Vector3 GetClosestEntryPointRelativeToObject(Vector3 objectsWorldPosition)
        {
            return (objectsWorldPosition - roomAEntryPoint.position).sqrMagnitude < (objectsWorldPosition - roomBEntryPoint.position).sqrMagnitude 
                ? roomAEntryPoint.position : roomBEntryPoint.position;
        }

        /// <summary>
        /// Returns the entry point transform relative to the Room provided. If setup correctly, the entry point will be in the same room as the one provided.
        /// </summary>
        /// <param name="room">Relative Room.</param>
        /// <returns>The entry point transform. NULL if invalid Room.</returns>
        public Transform GetDoorEntryPointRelativeToGivenRoom(Room room)
        {
            if (room == RoomA)
            {
                return roomAEntryPoint;
            }
            else if (room == RoomB)
            {
                return roomBEntryPoint;
            }
            else
            {
                return null;
            }
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