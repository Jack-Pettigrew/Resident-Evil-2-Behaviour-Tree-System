using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Combat;

namespace DD.Core.Items
{
    [CreateAssetMenu(menuName = "Items/Ammo Item", fileName = "AmmoItem", order = 3)]
    public class AmmoItem : ItemData
    {
        [field: Header("Equipment")]
        [field: SerializeField] public EquipmentType EquipmentType { private set; get; }

        public override void Use()
        {
            // Send to ItemCombiner to combine with weapon/ammo logic
        }
    }
}
