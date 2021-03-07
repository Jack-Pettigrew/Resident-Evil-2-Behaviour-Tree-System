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

        private float currentRotVel = 0;

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
            dir = dir.normalized;
            Vector3 vel = dir * 1.0f;
            transform.position += vel * Time.deltaTime;

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotVel, 1.0f);
        }
    }

}