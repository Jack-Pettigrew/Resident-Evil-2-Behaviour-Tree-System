using UnityEngine;

namespace DD.Core.Items
{
    [CreateAssetMenu(fileName = "HealingItemData", menuName = "Items/Healing Item", order = 1)]
    public class HealingItemData : ItemData
    {
        [Header("Healing Properties")]
        public float healAmount = 1;

        public override Item CreateItemInstance(int amountOfItem)
        {
            return new HealingItem(this, amountOfItem);
        }
    }
}
