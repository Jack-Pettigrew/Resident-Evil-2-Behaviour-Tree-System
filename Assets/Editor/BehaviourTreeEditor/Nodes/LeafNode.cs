using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public class LeafNode : Node
    {
        private string nodeName = DEFAULTNODENAME;
        private const string DEFAULTNODENAME = "Leaf Node";
        
        private Object nodeType = null;

        private DD.AI.BehaviourTreeSystem.Node leafNodeLogic = null;

        public LeafNode(Vector2 position)
        {
            NodeRect = new Rect(position, new Vector2(300, 100));
            nodeLinkInPos = Vector2.zero;
        }
        public LeafNode(Vector2 position, DD.AI.BehaviourTreeSystem.Node leafNodeLogic)
        {
            NodeRect = new Rect(position, new Vector2(300, 100));
            nodeLinkInPos = Vector2.zero;

            this.leafNodeLogic = leafNodeLogic;
        }

        public override void DrawNode(int windowID)
        {
            NodeRect = GUILayout.Window(windowID, NodeRect, DrawNodeContentCallback, nodeName);
        }

        protected override void DrawNodeContentCallback(int windowID)
        {
            nodeType = EditorGUILayout.ObjectField("Node Script", nodeType, typeof(MonoScript), false);

            if(nodeType == null)
            {
                EditorGUILayout.HelpBox("Ensure to assign a Behaviour this Node should excute when reached.", MessageType.Warning);
                nodeName = DEFAULTNODENAME;
            }
            else
            {
                nodeName = nodeType.name;
            }

            GUI.DragWindow(new Rect(0, 0, NodeRect.width, 20));
        }
    }
}