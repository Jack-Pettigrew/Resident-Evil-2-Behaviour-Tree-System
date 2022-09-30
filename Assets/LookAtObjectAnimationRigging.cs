using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtObjectAnimationRigging : MonoBehaviour
{
    private Rig rig;
    [SerializeField] private Transform lookTargetHelper;
    private GameObject targetObject;

    private float targetWeight;
    [SerializeField] private float lerpSpeed = 10.0f;
    
    void Awake()
    {
        rig = GetComponent<Rig>();
    }

    void Update()
    {
        if(rig.weight != targetWeight)
        {
            rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * lerpSpeed);
        }
    }

    private void LateUpdate() {
        if(targetObject)
        {   
            lookTargetHelper.position = targetObject.transform.position;
        }
    }

    public void EnableLook(GameObject gameObject)
    {
        targetObject = gameObject;

        targetWeight = 1f;
    }

    public void DisableLook(GameObject gameObject)
    {
        if(gameObject == targetObject)
        {
            targetObject = null;
            
            targetWeight = 0;
        }
    }
}
