using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTree
{
    public class BlackboardTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            Blackboard.AddToBlackboard("Spawn", FindObjectOfType<CharacterController>().transform);
        }
    }

}