using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Items;

namespace DD.Systems
{
    public class GlobalEvents : MonoBehaviour
    {
        public static Action<GameObject> OnInteract;
        public static Action<ItemData> OnPickupItem;
    }
}
