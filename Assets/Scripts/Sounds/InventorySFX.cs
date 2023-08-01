using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.InventorySystem;

[RequireComponent(typeof(AudioSource))]
public class InventorySFX : MonoBehaviour
{
    private Inventory inventory;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        Inventory.Instance.OnItemAdded += PlayItemAddSound;
    }

    private void OnDisable() {
        Inventory.Instance.OnItemAdded -= PlayItemAddSound;
    }

    public void PlayItemAddSound(ItemSlot itemSlot)
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        audioSource.PlayOneShot(audioClip);
    }
}
