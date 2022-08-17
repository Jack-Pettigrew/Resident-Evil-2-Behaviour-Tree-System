using System.Collections;
using System.Collections.Generic;
using DD.Core.Control;
using UnityEngine;

// The following is disgusting - please don't judge me, not enough coffee in the world could help me find a better solution right now

namespace DD.Systems.Room
{
    public class DoubleDoor : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public bool CanInteract { set; get; }
        [field: SerializeField] public bool IsLocked { private set; get; }

        [Header("Doors")]
        [SerializeField] private Door[] doors;

        public void Interact(Interactor interactor)
        {
            if(!CanInteract) return;

            if(IsLocked)
            {
                // Play Noise
                return;
            }
            
            foreach (Door door in doors)
            {
                if(door.IsChangingState) return;

                door.ResetRunningCoroutines();
                
                if(!door.IsOpen)
                {
                    door.OpenDoor(interactor.transform.position);
                }
                else
                {
                    door.CloseDoor();
                }
            }
        }
    }
}

// DoubleDoor
// has two doors
// opens both on interact