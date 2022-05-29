using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.Controllers
{
    public interface IAIMoveable
    {
        bool UpdatePath(Vector3 goalPosition);
        void Move();
    }
}
