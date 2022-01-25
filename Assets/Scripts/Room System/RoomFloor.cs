using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class RoomFloor : MonoBehaviour
    {
        [SerializeField] private Room ownerRoom;
        public Room OwnerRoom { private set { ownerRoom = value; } get { return ownerRoom; } }

        private void Start()
        {
            if (ownerRoom == null)
            {
                Debug.LogWarning(this + " doesn't have an associated Room reference.");
            }
        }

        public void SetOwnerRoom(Room newOwner)
        {
            gameObject.layer = LayerMask.NameToLayer("Room");

            OwnerRoom = newOwner;
        }
    }
}