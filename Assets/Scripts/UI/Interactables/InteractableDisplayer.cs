using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DD.Core.Control;

public class InteractableDisplayer : MonoBehaviour
{
    [SerializeField] private Interactor interactor;
    private RawImage image;
    private Vector3 positionOffset = Vector3.zero;

    private bool active = false;
    private GameObject targetGameObject = null;

    private void Awake() {
        image = GetComponent<RawImage>();
        image.enabled = false;
    }

    private void OnEnable() {
        interactor.OnNearestInteractableFound += DisplayInteractor;
        interactor.OnNearestInteractableLost += HideInteractor;
    }

    private void OnDisable() {
        interactor.OnNearestInteractableFound -= DisplayInteractor;
        interactor.OnNearestInteractableLost -= HideInteractor;
    }

    private void DisplayInteractor(IInteractable interactable, GameObject gameObject)
    {
        // Debug.Log("Displaying...");
        active = true;
        image.enabled = active;
        targetGameObject = gameObject;
        positionOffset = interactable.InteractionIconOffset;
        transform.position = gameObject.transform.position + gameObject.transform.rotation * positionOffset;
    }

    private void HideInteractor()
    {
        // Debug.Log("Hiding...");
        active = false;
        image.enabled = active;
        targetGameObject = null;
    }

    private void LateUpdate() {
        if(!active) return;

        // Null check for cases such as items being picked up + no delay in icon hiding
        if(targetGameObject == null)
        {
            HideInteractor();
            return;
        }
        
        transform.LookAt(Camera.main.transform);
        transform.position = targetGameObject.transform.position + targetGameObject.transform.rotation * positionOffset;
    }
}
