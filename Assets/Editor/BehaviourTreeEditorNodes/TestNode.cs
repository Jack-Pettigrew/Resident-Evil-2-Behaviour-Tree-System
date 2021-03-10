using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Editor.BehaviourTreeEditor
{
    public class TestNode : Node
    {
        public TestNode(Vector2 position)
        {
            nodeRect = new Rect(position, new Vector2(300, 200));
            nodeInPos = Vector2.zero;
            nodeOutPos = Vector2.zero;
        }

    }
}