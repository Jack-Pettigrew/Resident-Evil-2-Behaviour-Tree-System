using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudioSource : MonoBehaviour
{
    private float currentPitch = 1.0f;
    [SerializeField, Range(0f, 1.0f)] private float minRandomPitch, maxRandomPitch;
    
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioSource leftFoot, rightFoot;
    
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
}

public enum Foot {
    LEFT,
    RIGHT
}
