using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTree;

namespace DD.AI.Controllers
{
    public class AIController : MonoBehaviour
    {
        private Node rootNode;

        [SerializeField] private LayerMask layerMask;

        // Start is called before the first frame update
        void Start()
        {
            Sequence followSequence = new Sequence(new List<Node> { new MoveToNode(this), new JumpNode(this) });

            CanSeePlayerNode canSeePlayerNode = new CanSeePlayerNode(transform, 45.0f, 2.0f, layerMask);
            DeleteSelfNode deleteSelfNode = new DeleteSelfNode(transform);
            Sequence deleteSelfSequence = new Sequence(new List<Node> { canSeePlayerNode, deleteSelfNode });

            rootNode = new Selector(new List<Node> { deleteSelfSequence, followSequence });
        }

        // Update is called once per frame
        void Update()
        {
            rootNode.Evaluate();
        }

        public void Move(Vector3 dir)
        {
            transform.position = transform.position - (dir.normalized * 1.0f * Time.deltaTime);
        }
    }

}