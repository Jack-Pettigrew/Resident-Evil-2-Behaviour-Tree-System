using System;
using System.Collections;
using System.Collections.Generic;
using DD.Animation;
using UnityEngine;

namespace DD.Core.Combat
{
    public class Knife : Weapon
    {
        public event Action OnAttack;
        
        protected override void Awake() 
        {
            base.Awake();

            FindObjectOfType<PlayerAnimationController>()?.InitAnimationEvent(this);
        }
        
        public override void Attack()
        {
            OnAttack?.Invoke();
        }

        public override void UseWeaponAction()
        {
            // Nothing
        }

        public override void SubscribeAnimator(PlayerAnimationController animationController)
        {
            OnAttack += animationController.TriggerShoot;
        }

        public override void UnsubscribeAnimator(PlayerAnimationController animationController)
        {
            OnAttack -= animationController.TriggerShoot;
        }
    }
}
