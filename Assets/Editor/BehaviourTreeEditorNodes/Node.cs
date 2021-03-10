using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public class Node
    {
        protected Rect nodeRect;

        protected Vector2 nodeInPos;
        protected Vector2 nodeOutPos;

        protected List<Node> childNodes = new List<Node>();

        /// <summary>
        /// Draws this Node.
        /// </summary>
        public virtual void DrawNode(int windowID)
        {
            nodeRect = GUI.Window(windowID, nodeRect, DrawNodeCallback, new GUIContent("Test Node"));

            DrawLinksToChildren();
        }

        protected void DrawNodeCallback(int id)
        {
            GUI.DragWindow();
        }

        /// <summary>
        /// Draws the Link from this Node to it's children.
        /// </summary>
        public void DrawLinksToChildren()
        {
            if (childNodes.Count < 1)
                return;

            Handles.BeginGUI();
            Handles.color = Color.white;

            foreach (Node child in childNodes)
            {
                Handles.DrawLine(nodeOutPos, child.nodeInPos, 2.0f);
            }

            Handles.EndGUI();
        }

        public void AddChildren(Node[] nodes)
        {
            childNodes.AddRange(nodes);
        }

        public void RemoveChildren(Node[] nodes)
        {
            childNodes.RemoveAll(x => nodes.Contains(x));
        }
    }
}