using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public abstract class Node
    {
        // Window
        public Rect NodeRect { protected set; get; }

        /// <summary>
        /// Draws this Node Window (basically extention of onGUI).
        /// </summary>
        /// <param name="windowID">A unique ID for this window (e.g. for loop index).</param>
        public abstract void DrawNode(int windowID);

        /// <summary>
        /// Draws the contents of this Node Window.
        /// </summary>
        /// <param name="windowID">A unique ID for this window (e.g. for loop index).</param>
        protected abstract void DrawNodeContentCallback(int windowID);

        /// <summary>
        /// Sets the individual Node draggable area.
        /// </summary>
        protected void MakeDraggable()
        {
            GUI.DragWindow(new Rect(0, 0, NodeRect.width, 20));
        }

        /// <summary>
        /// Moves the Node by delta.
        /// </summary>
        /// <param name="delta">Amount to move by.</param>
        public void Drag(Vector2 delta)
        {
            NodeRect = new Rect(NodeRect.position + delta, NodeRect.size);
        }
    }
}