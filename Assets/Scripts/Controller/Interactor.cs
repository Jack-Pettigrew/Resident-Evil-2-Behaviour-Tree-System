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
        [SerializeField] private InteractorActorType interactorActorType;
        public InteractorActorType InteractorActorType { get { return interactorActorType; } }
        
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
                IInteractable tempInteractable = collider.GetComponent<IInteractable>();

                // This is a child object, get from parent
                if(tempInteractable == null)
                {
                    tempInteractable = collider.GetComponentInParent<IInteractable>();
                }
                
                if(tempInteractable != null && tempInteractable.CanInteract)
                {                    
                    float angle = Vector3.Angle(transform.forward, collider.transform.position - transform.position);
                    float dist = Vector3.Distance(transform.position, collider.transform.position);

                    if(angle < maxAngle && dist < closestDist)
                    {
                        closestDist = dist; 
                        interactable = tempInteractable;
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

    public enum InteractorActorType { Player, AI }
}
