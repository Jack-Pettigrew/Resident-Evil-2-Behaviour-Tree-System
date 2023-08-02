using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.Sensors
{
    public class SoundEmitter : MonoBehaviour
    {        
        [SerializeField] private float soundRadius = 5.0f;
        [SerializeField] private LayerMask soundLayer;

        private Collider[] colliders = null;
        
        /// <summary>
        /// Raises OnEmittedSound event with this components current position.
        /// </summary>
        [ContextMenu("Emit Sound")]
        public void EmitSound()
        {
            colliders = Physics.OverlapSphere(transform.position, soundRadius, soundLayer);
            
            foreach (Collider collider in colliders)
            {
                if(collider.TryGetComponent<AIHear>(out AIHear aiHear))
                {
                    aiHear.HearSound();
                }
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, soundRadius);
        }
    }
}

