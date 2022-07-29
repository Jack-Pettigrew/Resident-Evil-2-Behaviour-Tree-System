using DD.AI;
using DD.AI.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DD.Core.Control;

namespace DD.Systems.Room
{
    [RequireComponent(typeof(HingeJoint))]
    public class Door : MonoBehaviour, IInteractable
    {
        // STATE
        public bool CanInteract { set; get; }
        public bool IsOpen { private set; get; }

        [Header("Interaction")]
        public bool ignorePlayerCollision = false;
        public float hingeSpeed = 1.0f;
        [SerializeField] private float openDoorCooldown = 5.0f;
        private float closedTargetRotation;

        // Components
        [SerializeField] private HingeJoint hinge;

        // ROOMS
        [Header("Connecting Room A")]
        [SerializeField] private Room roomA;
        public Room RoomA { get { return roomA; } }
        [Tooltip("The point where characters can start entering the Door from.")] public Transform roomAEntryPoint;

        [Header("Connecting Room B")]
        [SerializeField] private Room roomB;
        public Room RoomB { get { return roomB; } }
        [Tooltip("The point where characters can start entering the Door from.")] public Transform roomBEntryPoint;

        // EVENTS
        [Header("Events")]
        [SerializeField] private UnityEvent openDoorEvent;
        [SerializeField] private UnityEvent closeDoorEvent;

        private void Awake() {
            CanInteract = true;

            if(!hinge)
            {
                hinge = GetComponent<HingeJoint>();

                if(!hinge)
                {
                    Debug.LogWarning($"{name} Door: No HingeJoint has been set. Door will not open or close!");
                }
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
            // Vector3.Dot(hinge.connectedBody.transform.right, transform.forward)
            
            if(!IsOpen)
            {
                OpenDoor(interactor.InteractorTransform.position);
            }
            else
            {
                CloseDoor();
            }
        }

        /// <summary>
        /// Uses the HingeJoint Motor to drive the door to it's offset.
        /// </summary>
        /// <param name="targetRotationOffset">The offset the door should be driven to.</param>
        /// <returns></returns>
        private IEnumerator DriveDoorMotorToOffset(float targetRotationOffset)
        {
            
            // Setup joint motor
            JointMotor motor = hinge.motor;

            // Determine motor speed direction
            if(targetRotationOffset == 0)
            {
                motor.targetVelocity = Mathf.DeltaAngle(transform.eulerAngles.y, 0) > 0 ? -hingeSpeed : hingeSpeed;
            }
            else
            {
                motor.targetVelocity = (transform.localRotation.eulerAngles.y - targetRotationOffset) > 0 ? -hingeSpeed : hingeSpeed;
            }
                        
            // Start motor
            hinge.motor = motor;
            hinge.useMotor = true;

            // TODO: Motor isn't stopping when it reaches 0 on closing (Mathf.Approximately might not be detecting when it's near 0)

            // Wait until motor has reached target angle
            while(!Mathf.Approximately(transform.localRotation.eulerAngles.y, (closedTargetRotation + targetRotationOffset)))
            {

                yield return null;
            }

            // Stop hinge motor
            hinge.useMotor = false;
        }

        /// <summary>
        /// Opens the Door.
        /// </summary>
        /// <returns></returns>
        public void OpenDoor(Vector3 openerPosition)
        {
            StartCoroutine(ManuallyOpenDoor(openerPosition));
        }

        private IEnumerator ManuallyOpenDoor(Vector3 openerPosition)
        {            
            IsOpen = true;
            // TODO: Play Door opening noise 
            
            ignorePlayerCollision = true;
            // TODO: ignore player collisions
            
            Debug.Log("Openning Door...");
                        
            // TODO: base open offset on interactor position and pass to function

            yield return DriveDoorMotorToOffset(
                Vector3.Dot(hinge.connectedBody.transform.right, openerPosition - hinge.connectedBody.transform.position) < 0 ? 90.0f : -90.0f
            );

            Debug.Log("Door fully open!");

            openDoorEvent?.Invoke();

            // TODO: start cooldown coroutine
            StartCoroutine(OpenDoorCooldown());
        }

        private IEnumerator OpenDoorCooldown()
        {
            float timer = openDoorCooldown;

            Debug.Log("Door cooldown...");

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            // TODO: start door close dynamic
            StartCoroutine(CloseDoorDynamic());
        }

        private IEnumerator CloseDoorDynamic()
        {
            Debug.Log("Closing Door...");
            
            yield return DriveDoorMotorToOffset(0.0f);
            
            Debug.Log("Door closed!");

            IsOpen = false;
            closeDoorEvent?.Invoke();
        }

        /// <summary>
        /// Closes the Door.
        /// </summary>
        /// <returns></returns>
        public void CloseDoor()
        {
            StartCoroutine(CloseDoorDynamic());
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