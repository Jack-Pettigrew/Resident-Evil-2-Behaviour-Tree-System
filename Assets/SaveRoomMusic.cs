using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;
using DD.AI.Controllers;
using DD.AI.States;

[RequireComponent(typeof(AudioSource))]
public class SaveRoomMusic : MonoBehaviour
{
    [SerializeField] private AudioClip songIntro, songLoop;
    private AudioSource audioSource;
    private float playDelay = 2.0f;

    public bool IsActive { private set; get; }
    private Coroutine musicCoroutine = null;

    [Tooltip("The room that should be checked to see if the music should be playing.")]
    [SerializeField] private Room conditionalRoom = null;

    [SerializeReference] private AIBehaviourTreeController behaviourTreeInspectable;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        RoomManager.OnPlayerRoomChanged += PlayerRoomUpdated;
    }

    private void OnDisable()
    {
        RoomManager.OnPlayerRoomChanged -= PlayerRoomUpdated;
    }

    private void PlayerRoomUpdated(Room updatedRoom)
    {
        if (behaviourTreeInspectable == null) return;

        // If this room AND MR. X not in attack state
            // Begin
        if(updatedRoom == conditionalRoom && (behaviourTreeInspectable.PeekAtBlackBoardVariable<MrXState>("State") != MrXState.ATTACKING))
        {
            IsActive = true;

            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
            }

            musicCoroutine = StartCoroutine(PlaySaveMusic());
        }

        // If not this room AND not currently active
            // fade out and stop
        if(updatedRoom != conditionalRoom && IsActive)
        {
            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
            }

            musicCoroutine = StartCoroutine(StopSaveMusic());
        }
    }

    private IEnumerator PlaySaveMusic()
    {
        yield return new WaitForSeconds(playDelay);

        audioSource.volume = 1.0f;
        audioSource.clip = songIntro;
        audioSource.loop = false;
        audioSource.Play();

        yield return new WaitForSeconds(songIntro.length);
        
        audioSource.clip = songLoop;
        audioSource.loop = true;
        audioSource.Play();
        musicCoroutine = null;
    }

    private IEnumerator StopSaveMusic()
    {
        if (!IsActive) yield break;

        while(audioSource.volume > 0.05f)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0.0f, Time.deltaTime);
            yield return null;
        }

        IsActive = false;
        audioSource.Stop();
    }
}
