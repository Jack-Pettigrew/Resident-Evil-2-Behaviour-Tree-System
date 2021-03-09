using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

namespace DD.AI.Controllers
{
    public class AIBeahviourTreeController : MonoBehaviour
    {
        private BehaviourTree behaviourTree = null;

        // Locomotion
        private float currentRotVel = 0;

        // FOV (seperate component that updates tree via event)
        [SerializeField] private LayerMask playerLayerMask;

        void Awake()
        {
            behaviourTree = new BehaviourTree();

            // Player Test
            Blackboard.AddToSharedBlackboard("PlayerTransform", FindObjectOfType<Core.Control.PlayerController>().transform);

            Sequence followSequence = new Sequence(behaviourTree, new List<Node> { new MoveToPlayerNode(behaviourTree, this), new JumpNode(behaviourTree,this) });

            CanSeePlayerNode canSeePlayerNode = new CanSeePlayerNode(behaviourTree,transform, 45.0f, 2.0f, playerLayerMask);
            DeleteSelfNode deleteSelfNode = new DeleteSelfNode(behaviourTree,transform);
            Sequence deleteSelfSequence = new Sequence(behaviourTree, new List<Node> { canSeePlayerNode, deleteSelfNode });

            Selector root = new Selector(behaviourTree,new List<Node> { deleteSelfSequence, followSequence });

            behaviourTree.SetBehaviourTree(root);
        }

        void Update()
        {
            behaviourTree.EvaluateTree();
        }

        public void Move(Vector3 dir)
        {
            dir = dir.normalized;
            Vector3 vel = dir * 1.0f;
            transform.position += vel * Time.deltaTime;

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotVel, 1.0f);
        }
    }

}