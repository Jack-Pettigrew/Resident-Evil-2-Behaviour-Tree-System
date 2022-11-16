using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabSwapWindow : ExtendedEditorWindow
{
    private static GameObject prefabToSwapTo = null;
    private static bool deleteOriginalOnSwap = true;
    private static bool keepParent = true;

    private static PrefabSwapWindow window = null;

    [MenuItem("Window/Prefab Swapper")]
    public static void ShowWindow()
    {
        if(window)
        {
            window.Close();
        }
        
        window = GetWindow<PrefabSwapWindow>("Prefab Swapper");
        Selection.selectionChanged += UpdateWindow;
    }

    private static void UpdateWindow()
    {
        window.Repaint();
    }

    private void OnGUI() {
        GUILayout.BeginVertical();

        prefabToSwapTo = (GameObject) EditorGUILayout.ObjectField("Asset to Swap with", prefabToSwapTo, typeof(GameObject), false);

        deleteOriginalOnSwap = EditorGUILayout.Toggle("Delete on Swap", deleteOriginalOnSwap);

        keepParent = EditorGUILayout.Toggle("Keep Parent", keepParent);

        if(Selection.count == 0 && prefabToSwapTo != null)
        {
            EditorGUILayout.HelpBox("Select scene objects to replace with the selected prefab abaove", MessageType.Info);
        }
        else
        {
            
            GUIContent content = new GUIContent();
            GUI.contentColor = Color.white;

            foreach (var gameObject in Selection.gameObjects)
            {
                content.text += $"{gameObject.name}\n";
                // EditorGUILayout.LabelField($"{gameObject.name}");
            }
            
            GUILayout.Box(content, GUILayout.MaxWidth(position.width), GUILayout.Height(250));

            if(GUILayout.Button("Swap selected for Prefab") && prefabToSwapTo != null)
            {
                SwapSelectedWithChosen();
            }
        }

        EditorGUILayout.HelpBox("For the best results, ensure the origin points between the selected object(s) and swapping object are the same", MessageType.Info);

        GUILayout.EndVertical();
    }

    private void OnDestroy() {
        Selection.selectionChanged -= UpdateWindow;
    }

    private void SwapSelectedWithChosen()
    {       
        GameObject[] selected = Selection.gameObjects;

        int undoID = Undo.GetCurrentGroup();

        foreach (GameObject selectedObject in selected)
        {
            if(keepParent)
            {
                GameObject instantiatedObject = (GameObject) PrefabUtility.InstantiatePrefab(prefabToSwapTo, selectedObject.transform.parent);
                instantiatedObject.transform.localPosition = selectedObject.transform.localPosition;
                instantiatedObject.transform.localRotation = selectedObject.transform.localRotation;

                Undo.RegisterCreatedObjectUndo(instantiatedObject, "Prefab swapper instantiation");
            }
            else
            {
                GameObject instantiatedObject = (GameObject) PrefabUtility.InstantiatePrefab(prefabToSwapTo);
                instantiatedObject.transform.position = selectedObject.transform.position;
                instantiatedObject.transform.rotation = selectedObject.transform.rotation;

                Undo.RegisterCreatedObjectUndo(instantiatedObject, "Prefab swapper instantiation");
            }

            if(deleteOriginalOnSwap)
            {
                Undo.DestroyObjectImmediate(selectedObject);
            }

            Undo.CollapseUndoOperations(undoID);
        }
    }
}