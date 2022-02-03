using UnityEngine;
using UnityEditor;
using DD.Core.Combat;

[CustomEditor(typeof(Attacker))]
public class AttackerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("This attack collider is triggered through animation events. Please ensure an Animation triggers the respective methods.", MessageType.Info);
    }
}