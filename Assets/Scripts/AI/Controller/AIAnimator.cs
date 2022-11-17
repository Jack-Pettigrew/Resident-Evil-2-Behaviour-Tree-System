using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Animation.RigEvents;

namespace DD.AI.Controllers
{
    public class AIAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator Animator { get{ return animator; } }

        // Animation Rigging Events
        [SerializeField] private AniRigEventSignalReceiver[] rigSignalReceivers;
        private Dictionary<int, AniRigEventSignalReceiver> rigSignalReceiverDictionary = new Dictionary<int, AniRigEventSignalReceiver>();
        
        private void Start() {
            if(!animator)
            {
                Debug.LogError($"{name} has no animator!");
            }

            foreach (AniRigEventSignalReceiver signalReceiver in rigSignalReceivers)
            {
                rigSignalReceiverDictionary.Add(signalReceiver.ReceiverHash, signalReceiver);
            }
        }

        public void PlayAndWait(string stateName, Action callback = null)
        {
            animator.CrossFade(stateName, 0.01f);
            StartCoroutine(WaitForAnimation(callback));
        }

        private IEnumerator WaitForAnimation(Action callback)
        {            
            yield return new WaitForSeconds(animator.GetNextAnimatorStateInfo(0).length);
            
            callback?.Invoke();
        }


        /// <summary>
        /// Send a Animation Rigging Event to the signal receiver with the corresponding label hash code.
        /// </summary>
        /// <param name="aniRigEventSignalReceiverLabelHash">The signal's label in hash code form.</param>
        /// <param name="eventType">The type of animation rigging event.</param>
        public void SendAnimRigEventSignal(int aniRigEventSignalReceiverLabelHash, AnimRigEventType eventType)
        {
            if(rigSignalReceiverDictionary.TryGetValue(aniRigEventSignalReceiverLabelHash, out AniRigEventSignalReceiver signalReceiver))
            {
                signalReceiver.ReceiveSignal(eventType);
            }
        }
    }
}
