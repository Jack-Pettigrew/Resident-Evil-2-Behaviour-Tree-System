using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Editor.BehaviourTreeEditor
{
    public class TestNode : Node
    {
        public TestNode(Vector2 position)
        {
            NodeRect = new Rect(position, new Vector2(300, 200));
        }

        public override void DrawNode(int windowID)
        {
            NodeRect = GUILayout.Window(windowID, NodeRect, DrawNodeContentCallback, "Test Node");
        }

        protected override void DrawNodeContentCallback(int windowID)
        {
            GUILayout.Label("Test Label");

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

            GUILayout.Label("This is the Node View");

            GUILayout.EndVertical();

            GUILayout.BeginVertical("box", GUILayout.MaxWidth(400), GUILayout.ExpandHeight(true));

            GUILayout.Label("This is the Side Panel");

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            MakeDraggable();
         }
    }
}