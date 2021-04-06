using System;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

namespace DD.AI.Controllers
{
    public interface IAIBehaviour
    {
        // EVENTS
        public Action<Transform> SetMoveTarget { get; set; }
        public Action<Vector3> MoveEvent { get; set; }

        // EVENT METHODS
        public Func<Transform> GetAITransform { get; set; }
        public Func<Transform> GetAIMoveTarget { get; set; }

        public Blackboard GetAIBlackboard();
    } 
}