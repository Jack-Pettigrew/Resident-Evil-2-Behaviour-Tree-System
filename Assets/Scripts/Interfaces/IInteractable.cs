using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Control
{
    public interface IInteractable
    {
        Vector3 InteractionIconOffset { set; get; }
        bool CanInteract { set; get; }
        void Interact(Interactor interactor);
    }
}
