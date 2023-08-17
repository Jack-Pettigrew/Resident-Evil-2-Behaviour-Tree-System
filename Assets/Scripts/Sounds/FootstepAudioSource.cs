using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudioSource : MonoBehaviour
{
    [SerializeField] private Transform comparisonTransform;

    private float currentPitch = 1.0f;
    [SerializeField, Range(0f, 1.0f)] private float minRandomPitch, maxRandomPitch;
    
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioSource leftFoot, rightFoot;

    private void LateUpdate()
    {
        if (Mathf.Abs((comparisonTransform.position - leftFoot.transform.position).y) <= 0.15f)
        {
            PlayFootstep(Foot.LEFT);
        }

        if (Mathf.Abs((comparisonTransform.position - rightFoot.transform.position).y) <= 0.15f)
        {
            PlayFootstep(Foot.RIGHT);
        }
    }

    public void PlayFootstep(Foot foot)
    {
        AudioSource audioSource = foot == Foot.LEFT ? leftFoot : rightFoot;

        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        currentPitch = Random.Range(minRandomPitch, maxRandomPitch);
        audioSource.pitch = currentPitch;
        audioSource.PlayOneShot(footstepSound);

        // Debug.Log(foot.ToString());
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(leftFoot.transform.position, comparisonTransform.position);
    //}
}

public enum Foot {
    LEFT,
    RIGHT
}
