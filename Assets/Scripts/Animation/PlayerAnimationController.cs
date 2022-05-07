using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.Core.Combat;
using UnityEngine.Animations.Rigging;

namespace DD.Animation
{
    public class PlayerAnimationController : AnimationController
    {
        // Input Manager
        private InputManager inputManager;
        
        [Header("Animation Layers")]
        [SerializeField] private int weaponAnimatorLayerID = 1;

        [Header("Animation Targets")]
        [SerializeField] private MultiAimConstraint aimTargetConstraint;
        
        private void Awake() {
            inputManager = FindObjectOfType<InputManager>();
        }
        
        private void Start()
        {
            // subscribe to all events you can find relating to the player
            foreach (IAnimatorEvent<PlayerAnimationController> animatorEvent in FindObjectsOfType<MonoBehaviour>().OfType<IAnimatorEvent<PlayerAnimationController>>())
            {
                animatorEvent.SubscribeAnimator(this);
            }
        }

        private void LateUpdate() {
            UpdateLayers();
            UpdateAim();
        }

        public void UpdateMovement()
        {
            animator.SetFloat("VelX", Mathf.Lerp(animator.GetFloat("VelX"), inputManager.InputDirection.x, Time.deltaTime * 10.0f));
            animator.SetFloat("VelY", Mathf.Lerp(animator.GetFloat("VelY"), inputManager.InputDirection.z, Time.deltaTime * 10.0f));
            animator.SetBool("isSprinting", inputManager.Sprint);
        }

        public void UpdateLayers()
        {            

            animator.SetLayerWeight(weaponAnimatorLayerID, EquipmentManager.Instance.ActiveWeapon != null ? 1.0f : 0.0f);
        }

        public void UpdateAim()
        {
            animator.SetBool("Aiming", inputManager.Aim);
            aimTargetConstraint.weight = (EquipmentManager.Instance.ActiveWeapon != null && inputManager.Aim) ? 1.0f : 0.0f;
        }

        public void TriggerShoot()
        {
            animator.SetTrigger("Shoot");
            animator.ResetTrigger("Shoot");
        }

        public void TriggerReload()
        {
            animator.SetTrigger("Reload");
            animator.ResetTrigger("Reload");

        }

        public void UpdateWeaponType()
        {
            if(EquipmentManager.Instance.ActiveWeapon == null)
            {
                animator.SetInteger("WeaponType", -1);
                return;
            }
            
            animator.SetInteger("WeaponType", (int) EquipmentManager.Instance.ActiveWeapon.WeaponType);
        }
    }
}
