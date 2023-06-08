using UnityEngine;

namespace DD.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomStartPosition : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            SetRandomAudioClipPosition();
        }

        public void SetRandomAudioClipPosition()
        {
            if(!audioSource.clip) return;

            audioSource.time = Random.Range(0.0f, audioSource.clip.length);
        }
    }
}

