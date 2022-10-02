using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtObjectAnimationRigging : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint[] multiAimConstraints;
    [SerializeField] private Transform lookTargetHelper;
    private GameObject targetObject;

    private float targetWeight;
    [SerializeField] private float lerpSpeed = 10.0f;
    
    void Awake()
    {
        if(multiAimConstraints == null)
        {
            multiAimConstraints = new MultiAimConstraint[0];
            Debug.LogWarning($"No MultiAimConstrains have been assigned for LookAtObjectAnimationRigging.", this);
        }
    }

    void Update()
    {        
        foreach (MultiAimConstraint constraint in multiAimConstraints)
        {
            if(constraint.weight != targetWeight)
            {
                constraint.weight = Mathf.Lerp(constraint.weight, targetWeight, Time.deltaTime * lerpSpeed);
            }            
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
