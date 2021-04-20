using DD.AI;
using DD.AI.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class Door : MonoBehaviour, IAIInteractable
    {
        [SerializeField] private Room roomA, roomB;
        public Room RoomA { get { return roomA; } }
        public Room RoomB { get { return roomB; } }

        public void Interact(IAIBehaviour ai)
        {
            // Get which side AI is on from colliders
            // Set AI's Room in AI.BB

            // Begin Door animation
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