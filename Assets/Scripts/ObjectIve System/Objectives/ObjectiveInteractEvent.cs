using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;

public class ObjectiveInteractEvent : MonoBehaviour, IInteractable
{
    [field: SerializeField] public bool CanInteract { get; set; } = true;
    [field: SerializeField] public Vector3 InteractionIconOffset { set; get; }

    public void Interact(Interactor interactor)
    {
        // UH OH - THIS SMELLS A LITTLE FISHY :O
        // Probably better done if components told the quest the associated quest to evaluate more directly
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + InteractionIconOffset, .1f);
    }
}
