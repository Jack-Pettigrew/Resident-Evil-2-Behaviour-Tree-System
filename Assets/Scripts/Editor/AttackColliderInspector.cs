using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackCollider))]
public class AttackColliderInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("This attack collider is triggered through animation events. Please ensure an Animation triggers the respective methods.", MessageType.Info);
    }
}