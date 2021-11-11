using System;
using UnityEngine;

namespace DD.AI.Controllers
{
    /// <summary>
    /// An Interface allowing the Behaviour Tree to interface with external components/methods without the hassle of references.
    /// </summary>
    public interface IAIBehaviour
    {
        // EVENTS - allow for decoupled code
        public Action<Vector3> MoveEvent { get; set; }

        // METHODS
        public Transform GetAITransform();
        public Animator GetAnimator();
    } 
}