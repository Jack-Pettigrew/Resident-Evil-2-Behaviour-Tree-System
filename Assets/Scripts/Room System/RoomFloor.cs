using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Systems.Room
{
    public class RoomFloor : MonoBehaviour
    {
        private Room ownerRoom;
        public Room OwnerRoom { private set { ownerRoom = value; } get { return ownerRoom; } }

        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Room");

            if (ownerRoom == null)
            {
                Debug.LogError(this + " doesn't have an associated Room reference.");
            }
        }

        public void SetOwnerRoom(Room newOwner)
        {
            ownerRoom = newOwner;
        }
    }
}