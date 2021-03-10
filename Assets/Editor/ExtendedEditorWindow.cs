using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    /// <summary>
    /// Wrapper around UnityEditor property drawing functions for easy property drawing without needing to write it yourself.
    /// As Explained by: GameDevGuide
    /// </summary>
    /// <param name="serializedProperty"></param>
    /// <param name="drawChildren"></param>
    protected void DrawProperties(SerializedProperty serializedProperty, bool drawChildren)
    {
        string lastPropPath = string.Empty;

        foreach (SerializedProperty property in serializedProperty)
        {
            // Drawing Collection
            if(property.isArray && property.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName);

                if(property.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(property, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            // Drawing normal Field
            else
            {
                if (!string.IsNullOrEmpty(lastPropPath) && property.propertyPath.Contains(lastPropPath))
                    continue;
                lastPropPath = property.propertyPath;
                EditorGUILayout.PropertyField(property, drawChildren);
            }
        }
    }

    protected void DrawField(string propName, bool relative)
    {
        if (relative && currentProperty != null)
        {
            EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propName), true);
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propName), true);
        }
    }
}
