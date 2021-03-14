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
        protected Vector2 nodeLinkInPos;

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

        protected void MakeDraggable()
        {
            GUI.DragWindow(new Rect(0, 0, NodeRect.width, 20));
        }
    }
}