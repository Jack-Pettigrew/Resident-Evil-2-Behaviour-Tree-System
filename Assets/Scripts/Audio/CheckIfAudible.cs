using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Audio
{
    /// <summary>
    /// Editted. Source found at https://gamedevbeginner.com/unity-audio-optimisation-tips/#voice_limits
    /// </summary>
    public class CheckIfAudible : MonoBehaviour
    {
        AudioListener audioListener;
        AudioSource audioSource;
        float distanceFromPlayer;

        void Start()
        {
            // Finds the Audio Listener and the Audio Source on the object
            audioListener = Camera.main.GetComponent<AudioListener>();
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        void Update()
        {
            distanceFromPlayer = Vector3.Distance(transform.position, audioListener.transform.position);

            if (distanceFromPlayer <= audioSource.maxDistance)
            {
                ToggleAudioSource(true);
            }
            else
            {
                ToggleAudioSource(false);
            }
        }

        void ToggleAudioSource(bool isAudible)
        {
            if (!isAudible && audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else if (isAudible && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
