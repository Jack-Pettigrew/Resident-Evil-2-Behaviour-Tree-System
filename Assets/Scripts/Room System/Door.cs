using DD.AI;
using DD.AI.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Systems.Room
{
    public class Door : MonoBehaviour
    {
        // STATE
        public bool IsOpen { private set; get; }

        // ROOMS
        [Header("Connecting Room")]
        [SerializeField] private Room roomA;
        public Room RoomA { get { return roomA; } }
        public Transform roomAEntryPoint;

        [Header("Connecting Room")]
        [SerializeField] private Room roomB;
        public Room RoomB { get { return roomB; } }
        public Transform roomBEntryPoint;

        // EVENTS
        [SerializeField] private UnityEvent openDoorEvent;
        [SerializeField] private UnityEvent closeDoorEvent;

        public bool OpenDoor()
        {
            openDoorEvent?.Invoke();
            return true;
        }

        public bool CloseDoor()
        {
            closeDoorEvent?.Invoke();
            return true;
        }

        /// <summary>
        /// Returns this Door's entry point that is closest to the given position.
        /// </summary>
        /// <param name="objectsWorldPosition">The position of the object in world space.</param>
        /// <returns>The closest entry point transform.</returns>
        public Transform GetEntryPointRelativeToObject(Vector3 objectsWorldPosition)
        {
            return (objectsWorldPosition - roomAEntryPoint.position).sqrMagnitude < (objectsWorldPosition - roomBEntryPoint.position).sqrMagnitude 
                ? roomAEntryPoint : roomBEntryPoint;
        }

        /// <summary>
        /// Returns this Door's entry point to be used as an exit point that is closest to the given position.
        /// </summary>
        /// <param name="objectsWorldPosition">The position of the object in world space.</param>
        /// <returns>The closest entry point transform.</returns>
        public Transform GetExitPointRelativeToObject(Vector3 objectsWorldPosition)
        {
            return (objectsWorldPosition - roomAEntryPoint.position).sqrMagnitude < (objectsWorldPosition - roomBEntryPoint.position).sqrMagnitude
                ? roomBEntryPoint : roomAEntryPoint;
        }

        /// <summary>
        /// Returns the entry point transform relative to the Room provided.
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