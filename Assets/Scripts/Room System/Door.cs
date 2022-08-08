using DD.AI;
using DD.AI.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DD.Core.Control;

namespace DD.Systems.Room
{
    [RequireComponent(typeof(Rigidbody))]
    public class Door : MonoBehaviour, IInteractable
    {
        // STATE
        public bool CanInteract { set; get; }
        public bool IsChangingState { private set; get; }
        public bool IsOpen { private set; get; }
        public bool IsLocked { private set; get; }

        [Header("Interaction")]
        public bool ignorePlayerCollision = false;
        public float doorSpeed = 3.0f;
        [SerializeField] private float openDoorCooldown = 5.0f;
        private float closedTargetRotation;
        [SerializeField, Range(0, 0.5f), Tooltip("The threshold of which the door is considered closed during it's angular Lerp.")]
        private float doorClosedThreshold = 0.1f;

        // Coroutine
        private Coroutine runningCoroutine;

        // Components
        [Header("Components"), SerializeField] private Transform hingeParentTransform;
        [SerializeField] private new Rigidbody rigidbody;

        // ROOMS
        [Header("Connecting Rooms")]
        [SerializeField] private Room roomA;
        public Room RoomA { get { return roomA; } }
        [Tooltip("The point where characters can start entering the Door from.")] public Transform roomAEntryPoint;
        [Space(10)]
        [SerializeField] private Room roomB;
        public Room RoomB { get { return roomB; } }
        [Tooltip("The point where characters can start entering the Door from.")] public Transform roomBEntryPoint;

        // EVENTS
        [Header("Events")]
        [SerializeField] private UnityEvent openDoorEvent;
        [SerializeField] private UnityEvent closeDoorEvent;

        private void Awake() {
            CanInteract = true;
            
            if(IsLocked)
            {
                ignorePlayerCollision = true;
            }

            if(!hingeParentTransform)
            {
                Debug.LogWarning($"Door '{name}' does not have a hinge parent assigned. Please assign a hinge parent to this Door.");
            }

            if(!rigidbody)
            {
                rigidbody = GetComponent<Rigidbody>();
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
            if(IsLocked || IsChangingState) 
            {
                return;
            }
            
            if(runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }
            
            if(!IsOpen)
            {
                OpenDoor(interactor.InteractorTransform.position);
            }
            else
            {
                CloseDoor();
            }
        }

        /*
        Slerp Door:
        - Remove all hinge related logic
        - Add rigidbody that only moves rotation on Y axis (toggle on Y axis lock when door is either fully open or shut and off or player is colliding with it)
        - define rotational limits
        - Manual open + close - Slerp rotation to rotation offset
        - Cancel any manual slerping if player touches door - start close cooldown if door is open and player is no longer touching it
        - Dynamic physic move - player only
            - while in collision with player, allow Y axis rotation and physics forces to push door up to limit
            - like above "start close cooldown if door is open and player is no longer touching it"
        - STOP COROUTINES FROM OVERLAPPING (e.g. closing door multiple times will stack coroutines) **********************
        */
        

        /// <summary>
        /// Uses the HingeJoint Motor to drive the door to it's offset.
        /// </summary>
        /// <param name="targetAngle">The angle the door should be driven to offset from 0 in degrees.</param>
        /// <returns></returns>
        private IEnumerator SlerpDoorToOffset(float targetAngle)
        {
            // Determine target rotation
            targetAngle = Mathf.Repeat(targetAngle, 360);

            // Debug.LogWarning(-targetAngle);

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
        public void OpenDoor(Vector3 openerPosition)
        {
            runningCoroutine = StartCoroutine(ManuallyOpenDoor(openerPosition));
        }

        private IEnumerator ManuallyOpenDoor(Vector3 openerPosition)
        {            
            IsOpen = true;
            IsChangingState = true;
            // TODO: Play Door opening noise
            
            ignorePlayerCollision = true;
            // TODO: ignore player collisions
            
            // Debug.Log("Openning Door...");
                        
            // TODO: base open offset on interactor position and pass to function

            // Debug.LogWarning(Vector3.Dot(transform.forward, (openerPosition - transform.position).normalized));

            // Rotate door towards the offset (-90 = +Z | 90 = -Z)
            yield return SlerpDoorToOffset(
                Vector3.Dot(transform.forward, (openerPosition - transform.position).normalized) < 0 ? 90.0f : -90.0f
            );

            // Debug.Log("Door fully open!");

            IsChangingState = false;
            openDoorEvent?.Invoke();

            runningCoroutine = StartCoroutine(OpenDoorCooldown());
        }

        private IEnumerator OpenDoorCooldown()
        {
            float timer = openDoorCooldown;

            // Debug.Log("Door cooldown...");

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
            // Debug.Log("Closing Door...");

            IsChangingState = true;
            
            yield return SlerpDoorToOffset(0.0f);
            
            // Debug.Log("Door closed!");

            IsOpen = false;
            IsChangingState = false;
            closeDoorEvent?.Invoke();
        }

        /// <summary>
        /// Closes the Door.
        /// </summary>
        /// <returns></returns>
        public void CloseDoor()
        {
            runningCoroutine = StartCoroutine(CloseDoorDynamic());
        }

        /// <summary>
        /// Returns the Door's entry point relative to the given world position.
        /// </summary>
        /// <param name="objectsWorldPosition">The position of the object in world space.</param>
        /// <returns>The closest entry point transform.</returns>
        public Transform GetEntryPointRelativeToObject(Vector3 objectsWorldPosition)
        {
            return (objectsWorldPosition - roomAEntryPoint.position).sqrMagnitude < (objectsWorldPosition - roomBEntryPoint.position).sqrMagnitude 
                ? roomAEntryPoint : roomBEntryPoint;
        }

        /// <summary>
        /// Returns this Door's exit point relative to the given world position.
        /// </summary>
        /// <param name="objectsWorldPosition">The position of the object in world space.</param>
        /// <returns>The closest entry point transform.</returns>
        public Transform GetExitPointRelativeToObject(Vector3 objectsWorldPosition)
        {
            return (objectsWorldPosition - roomAEntryPoint.position).sqrMagnitude < (objectsWorldPosition - roomBEntryPoint.position).sqrMagnitude
                ? roomBEntryPoint : roomAEntryPoint;
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