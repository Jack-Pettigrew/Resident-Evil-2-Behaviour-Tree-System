using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.Controllers
{
    public class AIAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator Animator { get{ return animator; } }
        
        private void Awake() {
            if(!animator)
            {
                Debug.LogError($"{name} has no animator!");
            }
        }

        public void PlayAndWait(string stateName, Action callback)
        {
            animator.CrossFade(stateName, 0.01f);
            StartCoroutine(WaitForAnimation(callback));
        }

        private IEnumerator WaitForAnimation(Action callback = null)
        {            
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            
            callback.Invoke();
        }
    }
}
