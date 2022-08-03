using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerPhysicsInteractor : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField, Min(0)]
    [Tooltip("Affects the strength of the collision force.")]
    private float collisionForceScalar = 1;

    [SerializeField] private ForceMode forceMode;

    private void Awake() {
        controller = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.rigidbody)
        {                        
            hit.rigidbody.AddForceAtPosition(controller.velocity * collisionForceScalar, hit.point, forceMode);
        }
    }
}
