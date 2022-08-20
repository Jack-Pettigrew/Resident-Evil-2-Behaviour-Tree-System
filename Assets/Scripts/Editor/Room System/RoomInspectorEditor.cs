using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DD.Systems.Room;

namespace DD.Editor.Rooms
{
    [CustomEditor(typeof(Room))]
    public class RoomInspectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Selection.objects.Length <= 1)
            {
                EditorGUILayout.HelpBox("Select RoomFloors to add to this Room.", MessageType.Info, true);
            }

            if (GUILayout.Button("Link Selected Floors"))
            {
                SetRoomFloors();
                Debug.Log($"Floors successfully linked to {target.name}");
            }
        }

        private void SetRoomFloors()
        {
            Room room = target as Room;

            room.SetRoomFloors(Selection.GetFiltered<RoomFloor>(SelectionMode.Unfiltered));
        }
    }
}