using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public abstract void Interact();
    }
}
