using System.Collections;
using System.Collections.Generic;

namespace DD.Core
{
    public interface IInteractable
    {
        bool CanInteract { set; get; }
        void Interact();
    }
}
