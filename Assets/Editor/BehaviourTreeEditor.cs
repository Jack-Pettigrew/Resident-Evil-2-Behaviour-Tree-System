using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public class BehaviourTreeEditor : ExtendedEditorWindow
    {
        private Event e = null;

        private List<Node> nodes = new List<Node>();

        [MenuItem("Window/Behaviour Tree Editor")]
        public static void ShowWindow()
        {
            BehaviourTreeEditor window = GetWindow<BehaviourTreeEditor>("Behaviour Tree Editor");

            // Use this for when you have asset files to open
            //window.serializedObject = new SerializedObject(dataObject);
        }

        private void OnGUI()
        {
            DrawNodes();

            e = Event.current;
            ProcessInput();

            // LAYOUT
            //EditorGUILayout.BeginHorizontal();

            //EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

            //EditorGUILayout.LabelField("This is the Node View");

            //EditorGUILayout.EndVertical();

            //EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(400), GUILayout.ExpandHeight(true));

            //EditorGUILayout.LabelField("This is the Side Panel");

            //EditorGUILayout.EndVertical();

            //EditorGUILayout.EndHorizontal();
        }

        private void DrawNodes()
        {
            BeginWindows();

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].DrawNode(i);
            }

            EndWindows();
        }

        private void ProcessInput()
        {
            if (e.type == EventType.ContextClick)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Node"), false, ContextCallback, null);
                menu.ShowAsContext();
            }
        }

        private void ContextCallback(object userData)
        {
            nodes.Add(new TestNode(new Vector2(e.mousePosition.x, e.mousePosition.y)));
        }
    }
}