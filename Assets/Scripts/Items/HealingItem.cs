using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;

namespace DD.Core.Items
{
    [CreateAssetMenu(fileName = "HealingItemData", menuName = "Items/Healing Item")]
    public class ItemHealing : ItemData
    {
        [Header("Healing Properties")]
        public float healAmount = 1;

        public override void Use()
        {
            /*
                The following makes me sick but I currently can't think of a generic way that doesn't destroy
                the way ItemData is used within the Inventory without specifying a dummy type.
            */
            FindObjectOfType<PlayerController>().GetComponent<Health>().Heal(healAmount);
        }
    }
}
