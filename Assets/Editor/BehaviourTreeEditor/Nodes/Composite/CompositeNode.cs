using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public abstract class CompositeNode : Node
    {
        // Links
        protected Vector2 nodeLinkOutPos;
        protected List<Node> childNodes = new List<Node>();

        protected void DrawNodeIcon(string path)
        {
            GUILayout.Box(AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture);
        }

        /// <summary>
        /// Draws the Link from this Node to it's children.
        /// </summary>
        public void DrawLinksToChildren()
        {
            throw new System.NotImplementedException();
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