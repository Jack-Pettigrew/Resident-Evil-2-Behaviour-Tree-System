using UnityEngine;

namespace DD.Core.Items
{
    [CreateAssetMenu(fileName = "HealingItemData", menuName = "Items/Healing Item")]
    public class HealingItem : ItemData
    {
        [Header("Healing Properties")]
        public float healAmount = 1;
    }
}
