using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.Controllers
{
    [RequireComponent(typeof(Animator))]
    public class AIAnimator : MonoBehaviour
    {
        private Animator animator;
        public Animator Animator { get; }
        
        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void PlayAndWait(string stateName, Action callback)
        {
            animator.SetTrigger(stateName);
            StartCoroutine(WaitForAnimation(callback));
        }

        private IEnumerator WaitForAnimation(Action callback = null)
        {            
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            
            callback.Invoke();
        }
    }
}
