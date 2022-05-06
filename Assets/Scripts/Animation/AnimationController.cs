using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Animation
{
    public abstract class AnimationController : MonoBehaviour
    {
        [SerializeField] protected Animator animator;

        public Animator GetAnimator()
        {
            return animator;
        }
    }

    public interface IAnimatorEvent<T> where T : AnimationController
    {
        void SubscribeAnimator(T animationController);
        void UnsubscribeAnimator(T animationController);
    }
}
