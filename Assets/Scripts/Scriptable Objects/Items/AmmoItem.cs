using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Items
{
    [CreateAssetMenu(fileName = "AmmoItem", menuName = "Items/Ammo Item")]
    public class AmmoItem : ItemData
    {
        [Header("Ammo Properties")]
        public AmmoType ammoType;
    }
}