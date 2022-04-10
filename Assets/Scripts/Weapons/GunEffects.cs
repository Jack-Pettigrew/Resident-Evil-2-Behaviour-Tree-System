using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Combat
{    
    [CreateAssetMenu(fileName = "gunEffect", menuName = "Effects/Weapon/Gun Effect")]
    public class GunEffects : WeaponEffects 
    {
        public ParticleSystem muzzleFlashEffect;
    }
}