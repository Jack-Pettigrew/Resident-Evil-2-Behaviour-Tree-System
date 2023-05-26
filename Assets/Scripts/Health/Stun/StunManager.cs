using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Core
{
    /// <summary>
    /// Tracks and manages stun 'gauge', invoking events when reaching thresholds.
    /// </summary>
    public class StunManager : MonoBehaviour
    {
        [field: ReadOnly, SerializeField] public bool IsStunned { get; private set; }

        // STUN VARIABLES
        [SerializeField, ReadOnly] private float currentStunAmount = 0.0f;
        [SerializeField, Min(0)] private float stunThreshold = 100.0f;

        // STUN  DEPLETION
        private bool depleteStun = false;
        [SerializeField, Min(1)] private float stunDepletionSpeed = 1.0f;

        // STUN COOLDOWN
        [SerializeField, Min(0)] private readonly float stunTimerLength = 5.0f;
        private float stunTimer = 0.0f;

        // EVENTS
        public UnityEvent OnStunned;
        public UnityEvent OnStunnedFinished;

        // Corountines
        private Coroutine stunCooldownCoroutine = null;

        private void Update()
        {
            if(depleteStun)
            {
                currentStunAmount -= Time.deltaTime * stunDepletionSpeed;

                if(currentStunAmount <= 0)
                {
                    currentStunAmount = 0;
                    depleteStun = false;
                }
            }
        }

        public void Stun(float stunAmount)
        {
            if(IsStunned) return;
            
            currentStunAmount += stunAmount;
            depleteStun = true;

            if(currentStunAmount >= stunThreshold)
            {
                IsStunned = true;
                depleteStun = false;
                currentStunAmount = 0;
                OnStunned?.Invoke();
                stunCooldownCoroutine = StartCoroutine(StunCooldown());
            }
        }

        private IEnumerator StunCooldown()
        {
            while(IsStunned)
            {
                stunTimer += Time.deltaTime;

                if(stunTimer >= stunTimerLength)
                {
                    IsStunned = false;
                    stunTimer = 0.0f;
                }
                
                yield return null;
            }
            
            ResetStun();
        }

        private void ResetStun()
        {
            StopCoroutine(stunCooldownCoroutine);
            currentStunAmount = 0.0f;
            depleteStun = false;
            IsStunned = false;
            OnStunnedFinished?.Invoke();
        }
    }
}
