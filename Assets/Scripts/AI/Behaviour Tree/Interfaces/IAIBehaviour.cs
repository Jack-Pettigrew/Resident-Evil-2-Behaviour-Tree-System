using System;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;
using DD.Systems.Room;

namespace DD.AI.Controllers
{
    /// <summary>
    /// An Interface allowing the Behaviour Tree to interface with external components/methods without the hassle of references.
    /// </summary>
    public interface IAIBehaviour
    {
        // EVENTS
        public Action<Vector3> MoveEvent { get; set; }

        // Retrival Events
        public Blackboard GetAIBlackboard();
        public Func<Transform> GetAITransform { get; set; }
        public Animator GetAnimator();
    } 
}