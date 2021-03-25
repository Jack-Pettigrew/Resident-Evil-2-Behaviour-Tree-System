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

        private bool isLinkingNodes = false;

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
            GUILayout.BeginHorizontal();

            NodeEditor();

            BlackboardInspector();

            GUILayout.EndHorizontal();

        }

        private void BlackboardInspector()
        {
            GUILayout.BeginVertical("box", GUILayout.MaxWidth(400), GUILayout.ExpandHeight(true));

            GUILayout.Label("This is the Side Panel");

            GUILayout.EndVertical();
        }

        private void NodeEditor()
        {
            GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (isLinkingNodes)
            {
                Vector2 start = selectedNode.NodeRect.center;
                Vector2 end = e.mousePosition;
                Vector2 startCurve = start + (end - start).normalized * 20;
                Vector2 endCurve = e.mousePosition + (start - end).normalized * 20;

                for (int i = 0; i < 3; i++)
                {
                    Handles.DrawBezier(start, end, startCurve, endCurve, Color.white, null, 1.0f);
                }

                Repaint();
            }

            DrawNodes();

            e = Event.current;

            if (e.isKey || e.isMouse || e.isScrollWheel)
            {
                ProcessInput();
            }

            GUILayout.EndVertical();
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

        /// <summary>
        /// Handle User Input
        /// </summary>
        private void ProcessInput()
        {
            // Linking
            if(isLinkingNodes)
            {
                isLinkingNodes = false;

                if(e.type == EventType.MouseDown)
                {
                    Node childNode = GetHoveredOverNode();

                    if(childNode != null && selectedNode is CompositeNode)
                    {
                        // TODO: Prevent Nodes linking to each other recursively (e.g. A -> B -> A)

                        (selectedNode as CompositeNode).AddChildren(new Node[] { childNode });
                    }
                }
            }

            // Right Click
            if (e.type == EventType.ContextClick)
            {
                HandleRightClick();
            }
        }

        private void HandleRightClick()
        {
            GenericMenu menu = new GenericMenu();

            selectedNode = GetHoveredOverNode();
            clickedOnNode = selectedNode != null ? true : false;

            if(clickedOnNode)
            {
                // INSIDE NODE CONTEXT MENU
                // Node Type context menu checks here
                // e.g. sequence node might have different context options

                if (selectedNode is CompositeNode)
                {
                    menu.AddItem(new GUIContent("Link Child"), false, NodeContextCallback, UserAction.LinkChild);
                    menu.AddSeparator("");
                }

                menu.AddItem(new GUIContent("Delete Node"), false, NodeContextCallback, UserAction.DeleteNode);
                menu.ShowAsContext();
                e.Use();
            }
            else
            {
                // OUTSIDE NODE CONTEXT MENU
                menu.AddItem(new GUIContent("Add Test Node"), false, NodeContextCallback, UserAction.AddTestNode);
                menu.AddItem(new GUIContent("Add Leaf Node"), false, NodeContextCallback, UserAction.AddLeafNode);
                menu.AddItem(new GUIContent("Add Sequence Node"), false, NodeContextCallback, UserAction.AddSequence);
                menu.AddItem(new GUIContent("Add Selector Node"), false, NodeContextCallback, UserAction.AddSelector);
                menu.ShowAsContext();
                e.Use();
            }
        }

        private void NodeContextCallback(object userData)
        {
            switch ((UserAction)userData)
            {
                case UserAction.LinkChild:
                    isLinkingNodes = true;
                    break;

                case UserAction.AddTestNode:
                    nodes.Add(new TestNode(e.mousePosition));
                    break;
                case UserAction.AddLeafNode:
                    nodes.Add(new LeafNode(e.mousePosition));
                    break;
                case UserAction.AddSequence:
                    nodes.Add(new SequenceNode(e.mousePosition));
                    break;
                case UserAction.AddSelector:
                    nodes.Add(new SelectorNode(e.mousePosition));
                    break;

                case UserAction.DeleteNode:
                    if(selectedNode != null)
                    {
                        nodes.Remove(selectedNode);
                    }
                    break;
            }
        }

        private Node GetHoveredOverNode()
        {
            foreach (Node node in nodes)
            {
                if (node.NodeRect.Contains(e.mousePosition))
                {
                    return node;
                }
            }

            return null;
        }
    }

    public enum UserAction { AddTestNode, AddLeafNode, AddSequence, AddSelector, LinkChild, DeleteNode }
}