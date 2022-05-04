using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.Core.Combat;

namespace DD.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WeaponSlotGameplaySelector : MonoBehaviour
    {        
        [SerializeField] private float fadeTime = 1.0f;
        [SerializeField] private float weaponSlotsVisibleTime = 2.0f;
        private CanvasGroup weaponSlotsCanvasGroup;

        private WeaponSlotUI[] weaponSlotUIs;
        
        // COROUTINE
        private Coroutine displayCoroutine;
        
        private void Start() {
            weaponSlotsCanvasGroup = GetComponent<CanvasGroup>();
            weaponSlotUIs = GetComponentsInChildren<WeaponSlotUI>();

            InputManager.Instance.OnQuickSlotChange += DisplayWeaponSlotViewer;
        }

        public void DisplayWeaponSlotViewer(WeaponSlot weaponSlot)
        {           
            if(displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            
            displayCoroutine = StartCoroutine(DisplayViewer(weaponSlot));
        }

        private IEnumerator DisplayViewer(WeaponSlot weaponSlot)
        {
            foreach (WeaponSlotUI weaponSlotUI in weaponSlotUIs)
            {
                weaponSlotUI.DrawWeaponSlot();
            }

            while (weaponSlotsCanvasGroup.alpha < 1.0f)
            {
                weaponSlotsCanvasGroup.alpha += (1 / fadeTime) * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(weaponSlotsVisibleTime);

            while (weaponSlotsCanvasGroup.alpha > 0.0f)
            {
                weaponSlotsCanvasGroup.alpha -= (1 / fadeTime) * Time.deltaTime;
                yield return null;
            }
        }
    }
}
