using System;
using UnityEngine;
using DD.Core.Control;

namespace DD.AI.Controllers
{
    /// <summary>
    /// The AI Interface for AI variables and logic decoupled from the Behaviour Tree.
    /// </summary>
    public interface IAIBehaviour
    {
        // METHODS
        public Transform GetAITransform();
        public AIAnimator GetAnimator();
        public Interactor GetInteractor();

        /// <summary>
        /// Returns the given component from the AI.
        /// </summary>
        /// <returns>The component if found; otherwise null if not.</returns>
        public T GetAIComponent<T>();
    } 
}