using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public abstract class Node
    {
        // Window
        public Rect NodeRect { protected set; get; }

        // Links
        protected Vector2 nodeLinkIn;
        protected Vector2 nodeLinkOut;
        protected List<Node> childNodes = new List<Node>();

        /// <summary>
        /// Draws this Node (Window) within the editor.
        /// </summary>
        /// <param name="windowID">A unique ID for this window (e.g. for loop index).</param>
        public abstract void DrawNode(int windowID);
        /// <summary>
        /// Draws the actual contents inside this Node (Window).
        /// </summary>
        /// <param name="windowID">A unique ID for this window (e.g. for loop index).</param>
        protected abstract void DrawNodeContentCallback(int windowID);

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
                Handles.DrawLine(nodeLinkOut, child.nodeLinkIn, 2.0f);
            }

            Handles.EndGUI();
        }

        /// <summary>
        /// Adds given Nodes as children to this Node.
        /// </summary>
        /// <param name="nodes">Nodes to add.</param>
        public void AddChildren(Node[] nodes)
        {
            childNodes.AddRange(nodes);
        }

        /// <summary>
        /// Removes the given Nodes as children from this Node.
        /// </summary>
        /// <param name="nodes">Nodes to remove.</param>
        public void RemoveChildren(Node[] nodes)
        {
            childNodes.RemoveAll(x => nodes.Contains(x));
        }
    }
}