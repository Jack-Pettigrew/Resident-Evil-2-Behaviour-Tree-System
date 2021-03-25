using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public abstract class CompositeNode : Node
    {

        protected List<Node> childNodes = new List<Node>();

        protected void DrawNodeIcon(string path)
        {
            GUILayout.Box(AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture);
        }

        /// <summary>
        /// Draws the Link from this Node to it's children.
        /// </summary>
        protected void DrawLinksToChildren()
        {
            if(childNodes.Count == 0)
            {
                return;
            }

            foreach (Node node in childNodes)
            {
                Vector2 start = NodeRect.center + Vector2.up * (NodeRect.height / 2);
                Vector2 end = node.NodeRect.center - Vector2.up * (node.NodeRect.height / 2);
                Vector2 startCurve = start + Vector2.up * 20;
                Vector2 endCurve = end - Vector2.up * 20;

                for (int i = 0; i < 3; i++)
                {
                    Handles.DrawBezier(start, end, startCurve, endCurve, Color.white, null, 2.0f);
                }
            }

            DrawChildLinkExtras();
        }

        /// <summary>
        /// Draws optional extras for Composite child links (e.g. use to draw the order of the node each node attached to a SequenceNode).
        /// </summary>
        protected virtual void DrawChildLinkExtras()
        {
            // Default: Does Nothing.
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