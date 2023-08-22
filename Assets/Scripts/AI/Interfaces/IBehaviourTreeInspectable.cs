using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.Controllers
{
    public interface IBehaviourTreeInspectable
    {
        public T PeekAtBlackBoardVariable<T>(string blackboardKey);
    }
}