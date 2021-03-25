using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public class LeafNode : Node
    {
        private const string DEFAULTNODENAME = "Leaf Node";
        private string nodeName = DEFAULTNODENAME;
        
        private Object nodeType = null;

        public LeafNode(Vector2 position)
        {
            NodeRect = new Rect(position, new Vector2(300, 100));
        }

        public override void DrawNode(int windowID)
        {
            NodeRect = GUILayout.Window(windowID, NodeRect, DrawNodeContentCallback, nodeName);
        }

        protected override void DrawNodeContentCallback(int windowID)
        {
            nodeType = EditorGUILayout.ObjectField("Node Script", nodeType, typeof(MonoScript), false);

            if (nodeType == null)
            {
                EditorGUILayout.HelpBox("Ensure to assign a Behaviour this Node should excute when reached.", MessageType.Warning);
                nodeName = DEFAULTNODENAME;
            }
            else
            {
                nodeName = nodeType.name;
            }

            MakeDraggable();
        }
    }
}