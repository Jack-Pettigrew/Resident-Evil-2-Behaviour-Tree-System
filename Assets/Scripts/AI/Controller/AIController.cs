using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTree;

namespace DD.AI.Controllers
{
    public class AIController : MonoBehaviour
    {
        private Node rootNode;

        // Start is called before the first frame update
        void Start()
        {
            rootNode = new Sequence(new List<Node> { new MoveToNode(this), new JumpNode(this) });
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