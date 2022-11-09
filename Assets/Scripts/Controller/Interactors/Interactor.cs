using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core;

namespace DD.Core.Control
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private float range = 1.0f;
        [SerializeField] private float maxAngle = 90.0f;

        // Interactor Info
        public Transform InteractorTransform { private set; get; }
        [SerializeField] private InteractorType interactorType;
        public InteractorType InteractorType { get { return interactorType; } }
        
        private void Awake() 
        {
            InteractorTransform = transform;
        }
        
        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Interact();
            }
        }
        
        private void Interact()
        {
            IInteractable interactable = FindNearestInteractable();
            interactable?.Interact(this);
        }

        private IInteractable FindNearestInteractable()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            IInteractable interactable = null;
            float closestDist = range;

            foreach (Collider collider in colliders)
            {
                IInteractable foundInteractable = collider.GetComponent<IInteractable>();

                // This is a child object, get from parent
                if(foundInteractable == null)
                {
                    foundInteractable = collider.GetComponentInParent<IInteractable>();
                }
                
                if(foundInteractable != null && foundInteractable.CanInteract)
                {                    
                    Vector3 closestPoint = collider.ClosestPoint(transform.position);
                    float angle = Vector3.Angle(transform.forward, closestPoint - transform.position);
                    float dist = Vector3.Distance(transform.position, closestPoint);

                    if(angle < maxAngle && dist < closestDist)
                    {
                        closestDist = dist; 
                        interactable = foundInteractable;
                    }
                }
            }

            return interactable;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, maxAngle, 0) * (transform.forward * range));
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -maxAngle, 0) * (transform.forward * range));
        }
    }

    public enum InteractorType { Player, AI }
}