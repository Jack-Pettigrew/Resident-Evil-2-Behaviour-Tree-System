using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Selector : Node
    {
        protected List<Node> nodes = new List<Node>();

        /*
         * List of nodes to select from
         * 
         * Go through each of the conditions for each node 
         * Choose the node whos condition is true first
         * if all conditions fail, selector node fails too
         * 
         * 
         * Conditions:
         *      - unique Node with bool logic? (<-)
         *          - Condition Node (essentially a 'gate node'):
         *              - If true executes the next Node and returns true, otherwise returns false
         *          
         *      - Apart of the next node? (wouldn't make sense really, a leaf node shouldn't be responsible for whether it should execute or not)
         *          - UNLESS leaf nodes like melee return false on a distToPlayer check before actually doing a melee? False = try the next node
         *      
         */


        public Selector(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            // Process all until Success
            foreach (Node node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        return NodeState.RUNNING;
                    case NodeState.SUCCESSFUL:
                        return NodeState.SUCCESSFUL;
                        
                    case NodeState.FAILED:
                    default:
                        break;
                }
            }

            return NodeState.FAILED;
        }
    }
}