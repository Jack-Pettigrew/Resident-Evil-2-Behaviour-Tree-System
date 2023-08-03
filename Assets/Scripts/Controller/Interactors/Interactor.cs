using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core;
using DD.Systems;

namespace DD.Core.Control
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private float range = 1.0f;
        [SerializeField] private float maxAngle = 90.0f;

        // [SerializeField] private float nearestInteractablesPolling = 0.2f;

        // Interactable-GameObject Tuple
        private (IInteractable interactable, GameObject interactableGameObject) nearestTarget = (null, null);

        // Interactor Info
        public Transform InteractorTransform { private set; get; }
        [SerializeField] private InteractorType interactorType;
        public InteractorType InteractorType { get { return interactorType; } }

        // Coroutines
        private Coroutine coroutine = null;

        // Event
        public Action<IInteractable, GameObject> OnNearestInteractableFound;
        public Action OnNearestInteractableLost;

        private void Awake()
        {
            InteractorTransform = transform;
        }

        private void Start() {
            coroutine = StartCoroutine(FindNearestInteractable());
        }

        private void Update()
        {   
            if (Input.GetKeyDown(KeyCode.F))
            {
                Interact();
            }
        }

        private void Interact()
        {
            // Need to call event before interacting as depending in the interactable's logic it could be destroyed

            if (nearestTarget != (null, null))
            {
                GlobalEvents.OnInteract?.Invoke(nearestTarget.interactableGameObject);
            }

            nearestTarget.interactable?.Interact(this);
        }

        private IEnumerator FindNearestInteractable()
        {
            while (true)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, range);
                float closestDist = range;
                IInteractable nearestInteractable = null;
                GameObject nearestInteractableGameobject = null;

                foreach (Collider collider in colliders)
                {
                    IInteractable foundInteractable = collider.GetComponent<IInteractable>();

                    // This is a child object, get from parent
                    if (foundInteractable == null)
                    {
                        foundInteractable = collider.GetComponentInParent<IInteractable>();
                    }

                    if (foundInteractable != null && foundInteractable.CanInteract)
                    {
                        Vector3 closestPoint = collider.ClosestPoint(transform.position);
                        float angle = Vector3.Angle(transform.forward, closestPoint - transform.position);
                        float dist = Vector3.Distance(transform.position, closestPoint);

                        if (angle < maxAngle && dist < closestDist)
                        {
                            closestDist = dist;

                            nearestInteractable = foundInteractable;
                            nearestInteractableGameobject = collider.gameObject;
                        }
                    }
                }

                // Found 
                if(nearestInteractable != null && nearestInteractableGameobject != null && nearestTarget != (nearestInteractable, nearestInteractableGameobject))
                {
                    nearestTarget = (nearestInteractable, nearestInteractableGameobject);
                    OnNearestInteractableFound?.Invoke(nearestInteractable, nearestInteractableGameobject);
                }
                else if(nearestInteractable == null && nearestInteractableGameobject == null && nearestTarget != (null, null) ) {
                    nearestTarget = (null, null);
                    OnNearestInteractableLost?.Invoke();
                }

                yield return null;
                // yield return new WaitForSeconds(nearestInteractablesPolling);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, maxAngle, 0) * (transform.forward * range));
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -maxAngle, 0) * (transform.forward * range));
        }
    }

    public enum InteractorType { Player, AI }
}
