using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Combat
{   
    [CreateAssetMenu(fileName = "weaponEffect", menuName = "Effects/Weapon/Weapon Effect")]
    public class WeaponEffects : ScriptableObject 
    {
        public ParticleSystem hitEffect;
    }
}
