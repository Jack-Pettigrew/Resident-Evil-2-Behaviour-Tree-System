using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Combat;

namespace DD.Core.Items
{
    [CreateAssetMenu(fileName = "EquipmentItem", menuName = "Items/Equipment Item", order = 2)]
    public class EquipmentItem : ItemData
    {
        [field: Header("Equipment")]
        [field: SerializeField] public EquipmentType EquipmentType { private set; get; }

        public override void Use()
        {
            EquipmentManager.Instance.OpenWeaponSlotPicker(this);
        }
    }
}
