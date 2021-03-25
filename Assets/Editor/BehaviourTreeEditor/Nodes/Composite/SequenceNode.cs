using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(Vector2 position)
        {
            NodeRect = new Rect(position, new Vector2(100, 100));
        }

        public override void DrawNode(int windowID)
        {
            NodeRect = GUILayout.Window(windowID, NodeRect, DrawNodeContentCallback, "Sequence");

            DrawLinksToChildren();
        }

        protected override void DrawChildLinkExtras()
        {
        }

        protected override void DrawNodeContentCallback(int windowID)
        {
            DrawNodeIcon("Assets/Editor/BehaviourTreeEditor/Resources/Sequence.psd");

            MakeDraggable();
        }
    }
}