using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Combat;

namespace DD.Core.Items
{
    [CreateAssetMenu(menuName = "Items/Ammo Item", fileName = "AmmoItem", order = 3)]
    public class AmmoItem : EquipmentItem
    {   
        public override void Use()
        {
            // Send to ItemCombiner to combine with weapon/ammo logic
        }
    }
}
