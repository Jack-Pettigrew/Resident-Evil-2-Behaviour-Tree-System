using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DD.Editor.BehaviourTreeEditor
{
    public class BehaviourTreeEditor : ExtendedEditorWindow
    {
        private Event e = null;

        private bool clickedOnNode = false;
        private Node selectedNode = null;
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

            

            // Editor Window Layout
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
            // Right Click
            if (e.type == EventType.ContextClick)
            {
                HandleRightClick();
            }
        }

        private void HandleRightClick()
        {
            GenericMenu menu = new GenericMenu();

            selectedNode = null;
            clickedOnNode = false;
            foreach (Node node in nodes)
            {
                if (node.NodeRect.Contains(e.mousePosition))
                {
                    clickedOnNode = true;
                    selectedNode = node;
                    break;
                }
            }

            if(clickedOnNode)
            {
                // INSIDE NODE CONTEXT MENU
                // Node Type context menu checks here

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, UserAction.DeleteNode);
                menu.ShowAsContext();
                e.Use();
            }
            else
            {
                // OUTSIDE NODE CONTEXT MENU
                menu.AddItem(new GUIContent("Add Test Node"), false, ContextCallback, UserAction.AddTestNode);
                menu.AddItem(new GUIContent("Add Leaf Node"), false, ContextCallback, UserAction.AddLeafNode);
                menu.AddItem(new GUIContent("Add Sequence Node"), false, ContextCallback, UserAction.AddSequence);
                menu.ShowAsContext();
                e.Use();

            }
        }

        private void ContextCallback(object userData)
        {
            switch ((UserAction)userData)
            {
                case UserAction.AddTestNode:
                    nodes.Add(new TestNode(e.mousePosition));
                    break;
                case UserAction.AddLeafNode:
                    nodes.Add(new LeafNode(e.mousePosition));
                    break;
                case UserAction.AddSequence:
                    nodes.Add(new SequenceNode(e.mousePosition));
                    break;
                case UserAction.DeleteNode:
                    if(selectedNode != null)
                    {
                        nodes.Remove(selectedNode);
                    }
                    break;
            }
        }
    }

    public enum UserAction { AddTestNode, AddLeafNode, AddSequence, DeleteNode }
}