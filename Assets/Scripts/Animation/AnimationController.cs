using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Animation
{
    public abstract class AnimationController<T>: MonoBehaviour where T : AnimationController<T>
    {
        [SerializeField] protected Animator animator;

        public Animator GetAnimator()
        {
            return animator;
        }

        /// <summary>
        /// Initialises the given Animation Event.
        /// </summary>
        /// <param name="animationEvent">The animation event.</param>
        public void InitAnimationEvent(IAnimatorEvent<T> animationEvent)
        {
            animationEvent.SubscribeAnimator((T) this);
        }

        /// <summary>
        /// Terminates the given Animation Event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="animationEvent"></param>
        public void TerminateAnimationEvent(IAnimatorEvent<T> animationEvent)
        {
            animationEvent.UnsubscribeAnimator((T) this);
        }
    }

    public interface IAnimatorEvent<T> where T : AnimationController<T>
    {
        void SubscribeAnimator(T animationController);
        void UnsubscribeAnimator(T animationController);
    }
}
