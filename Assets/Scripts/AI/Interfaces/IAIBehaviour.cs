using System;
using UnityEngine;

namespace DD.AI.Controllers
{
    // LISKOV: This interface forces all AI controllers to implement these functions even if the BT doesn't necessarily need them... not sure how to get around this currently

    /// <summary>
    /// The AI Interface for AI variables and logic decoupled from the Behaviour Tree.
    /// </summary>
    public interface IAIBehaviour
    {
        // EVENTS - allow for decoupled logic
        public Action<Vector3> MoveEvent { get; set; }

        // METHODS
        public Transform GetAITransform();
        public AIAnimator GetAnimator();
    } 
}