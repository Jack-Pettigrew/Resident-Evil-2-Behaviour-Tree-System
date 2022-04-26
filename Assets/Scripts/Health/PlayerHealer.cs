using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;

namespace DD.Core
{
    public class PlayerHealer : MonoBehaviour
    {
        public static PlayerHealer Instance;

        [SerializeField] private Health playerHealth;

        private void Awake() {
            Instance = this;
        }

        public void SetPlayerHealth(Health healthComponent) => playerHealth = healthComponent;

        public void ConsumeHealItem(HealingItem healingItem)
        {
            playerHealth.Heal(healingItem.HealAmount);
        }
    }
}