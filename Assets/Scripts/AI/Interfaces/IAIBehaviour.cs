using System;
using UnityEngine;

namespace DD.AI.Controllers
{
    /// <summary>
    /// The AI Interface for AI variables and logic decoupled from the Behaviour Tree.
    /// </summary>
    public interface IAIBehaviour
    {
        // EVENTS - allow for decoupled logic
        public Action<Vector3> MoveEvent { get; set; }

        // METHODS
        public Transform GetAITransform();
        public Animator GetAnimator();
    } 
}