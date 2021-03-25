using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(Vector2 position)
        {
            NodeRect = new Rect(position, new Vector2(100, 100));
        }

        public override void DrawNode(int windowID)
        {
            NodeRect = GUILayout.Window(windowID, NodeRect, DrawNodeContentCallback, "Selector");

            DrawLinksToChildren();
        }

        protected override void DrawNodeContentCallback(int windowID)
        {
            DrawNodeIcon("Assets/Editor/BehaviourTreeEditor/Resources/Selector.psd");

            MakeDraggable();
        }
    }

}