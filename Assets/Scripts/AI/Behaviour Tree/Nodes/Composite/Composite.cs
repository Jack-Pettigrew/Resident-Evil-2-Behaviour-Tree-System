using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public abstract class Composite : Node
    {
        /// <summary>
        /// All the connected child nodes this Composite branches down to.
        /// </summary>
        protected List<Node> childNodes;


        public Composite(BehaviourTree behaviourTree, List<Node> childNodes) : base(behaviourTree)
        {
            this.childNodes = childNodes;
        }
    }
}