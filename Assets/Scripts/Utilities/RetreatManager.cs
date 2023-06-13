using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class RetreatManager : MonoBehaviour
    {
        private Blackboard blackboard;

        [SerializeField] private GameObject retreatPoint;

        // Start is called before the first frame update
        void Start()
        {
            blackboard = GetComponent<AIBehaviourTreeController>().BehaviourTree.Blackboard;
            blackboard.AddToBlackboard("retreatPoint", retreatPoint.transform.position);
            blackboard.AddToBlackboard("retreat", false);
        }

        public void TriggerRetreat()
        {
            blackboard.UpdateBlackboardVariable("retreat", true);
        }

        public void StopRetreat()
        {
            blackboard.UpdateBlackboardVariable("retreat", false);
        }
    }
}
