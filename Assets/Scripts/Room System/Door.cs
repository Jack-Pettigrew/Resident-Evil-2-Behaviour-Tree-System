using DD.AI;
using DD.AI.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DD.Core.Control;

namespace DD.Systems.Room
{
    public class Door : MonoBehaviour, IInteractable
    {
        [Header("Door Sibling")]
        [SerializeField] private bool openSiblingInUnison = false;
        [SerializeField] private Door doorSibling;
        
        // STATE
        [field: Header("Interaction")]
        [field: SerializeField] public bool CanInteract { set; get; }
        [field: SerializeField] public bool IsLocked { private set; get; }
        public bool IsChangingState { private set; get; }
        public bool IsOpen { private set; get; }

        public float doorSpeed = 3.0f;
        [SerializeField] private float openDoorCooldown = 5.0f;
        private float closedTargetRotation;
        [SerializeField, Range(0, 0.5f), Tooltip("The threshold of which the door is considered closed during it's angular Lerp.")]
        private float doorClosedThreshold = 0.1f;

        // Coroutine
        private Coroutine runningCoroutine;

        // Components
        [Header("Components")]
        private Transform hingeParentTransform;

        // ROOMS
        [Header("Connecting Rooms")]
        [SerializeField] protected Room roomA;
        public Room RoomA { get { return roomA; } }
        [Tooltip("The point where characters can start entering the Door from.")] public Transform roomAEntryPoint;
        [Space(10)]
        [SerializeField] protected Room roomB;
        public Room RoomB { get { return roomB; } }
        [Tooltip("The point where characters can start entering the Door from.")] public Transform roomBEntryPoint;

        // EVENTS
        [Header("Events")]
        [SerializeField] protected UnityEvent openDoorEvent;
        [SerializeField] protected UnityEvent closeDoorEvent;

        private void Awake() {            
            hingeParentTransform = transform.parent;

            if(!hingeParentTransform)
            {
                Debug.LogWarning($"Door '{name}' does not have a hinge parent assigned. Please assign a parent to act as a hinge for this Door.");
            }
        }

        private void Start() {
            closedTargetRotation = transform.localRotation.eulerAngles.y;
        }

        /// <summary>
        /// Contextually Opens/Closes the door.
        /// </summary>
        public void Interact(Interactor interactor)
        {
            if(!CanInteract || IsChangingState)
            {
                return;
            }

            if (IsLocked)
            {
                // play locked noise
                return;
            }
            
            ResetRunningCoroutines();
            
            if(!IsOpen)
            {
                OpenDoor(interactor.InteractorTransform.position);
            }
            else
            {
                CloseDoor();
            }
        }

        public void ResetRunningCoroutines()
        {
            if(runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
        }        

        /// <summary>
        /// Uses the HingeJoint Motor to drive the door to it's offset.
        /// </summary>
        /// <param name="targetAngle">The angle the door should be driven to offset from 0 in degrees.</param>
        /// <returns></returns>
        private IEnumerator SlerpDoorToOffset(float targetAngle)
        {
            // Determine target rotation
            targetAngle = Mathf.Repeat(targetAngle, 360);

            // Wait until door has rotated to target angle
            while(Mathf.Abs(Mathf.DeltaAngle(hingeParentTransform.localEulerAngles.y, targetAngle)) > doorClosedThreshold)
            {
                yield return new WaitForFixedUpdate();

                hingeParentTransform.localEulerAngles = Vector3.up * Mathf.LerpAngle(hingeParentTransform.localEulerAngles.y, targetAngle, Time.deltaTime * doorSpeed);
            }
        }

        /// <summary>
        /// Opens the Door.
        /// </summary>
        /// <returns></returns>
        public virtual void OpenDoor(Vector3 openerPosition, bool siblingInduced = false)
        {            
            if(!siblingInduced && openSiblingInUnison && doorSibling)
            {
                doorSibling.OpenDoor(openerPosition, true);
            }
            
            runningCoroutine = StartCoroutine(ManuallyOpenDoor(openerPosition));
        }

        private IEnumerator ManuallyOpenDoor(Vector3 openerPosition)
        {            
            IsOpen = true;
            IsChangingState = true;
                        
            // Rotate door towards the offset (-90 = +Z | 90 = -Z)
            yield return SlerpDoorToOffset(
                Vector3.Dot(transform.forward, (openerPosition - transform.position).normalized) < 0 ? 90.0f : -90.0f
            );

            IsChangingState = false;
            openDoorEvent?.Invoke();

            runningCoroutine = StartCoroutine(OpenDoorCooldown());
        }

        private IEnumerator OpenDoorCooldown()
        {
            float timer = openDoorCooldown;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            // TODO: start door close dynamic
            runningCoroutine = StartCoroutine(CloseDoorDynamic());
        }

        private IEnumerator CloseDoorDynamic()
        {
            IsChangingState = true;
            
            yield return SlerpDoorToOffset(0.0f);
            
            IsOpen = false;
            IsChangingState = false;
            closeDoorEvent?.Invoke();
        }

        /// <summary>
        /// Closes the Door.
        /// </summary>
        /// <returns></returns>
        public virtual void CloseDoor(bool siblingInduced = false)
        {
            if(!siblingInduced && openSiblingInUnison && doorSibling)
            {
                doorSibling.CloseDoor(true);
            }
            
            runningCoroutine = StartCoroutine(CloseDoorDynamic());
        }

        /// <summary>
        /// Returns the Door's entry point relative to the given world position.
        /// </summary>
        /// <param name="gameObject">The game object in question.</param>
        /// <returns>The closest entry point transform.</returns>
        public Transform GetEntryPointRelativeToObject(GameObject gameObject)
        {
            Room roomOfObject = RoomManager.GetRoomOfObject(gameObject);

            if (roomOfObject == RoomA) return roomAEntryPoint;

            if(roomOfObject == RoomB) return roomBEntryPoint;

            return null;
        }

        /// <summary>
        /// Returns this Door's exit point relative to the given world position.
        /// </summary>
        /// <param name="gameObject">The game object in question.</param>
        /// <returns>The closest entry point transform.</returns>
        public Transform GetExitPointRelativeToObject(GameObject gameObject)
        {
            Room roomOfObject = RoomManager.GetRoomOfObject(gameObject);

            if (roomOfObject == RoomA) return roomBEntryPoint;

            if(roomOfObject == RoomB) return roomAEntryPoint;

            return null;
        }

        private void OnDrawGizmosSelected()
        {
            // Draw lines to associated Rooms
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