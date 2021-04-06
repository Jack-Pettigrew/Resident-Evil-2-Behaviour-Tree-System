using System;
using UnityEngine;

namespace DD.AI.Controllers
{
    public interface IAIBehaviour
    {
        public Action<Transform> SetMoveTarget { get; set; }
        public Action<Vector3> MoveEvent { get; set; }
    } 
}