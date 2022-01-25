using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using DD.Systems.Room;

namespace DD.Editor.Rooms
{
    [CustomEditor(typeof(Door))]
    public class DoorInspectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            Door door = target as Door;

            EditorGUILayout.HelpBox("When Auto Linking, ensure the appropriate entry points can reach the desired RoomFloors.", MessageType.Info, true);

            if(GUILayout.Button("Auto-link Connecting Rooms"))
            {
                AutoLinkConnectingRooms();
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Automatically links the Door's connecting Rooms based on Raycasts from entry points to nearby RoomFloors.
        /// </summary>
        private void AutoLinkConnectingRooms()
        {
            Door door = target as Door;

            Room roomA = RoomManager.GetRoomOfObject(door.roomAEntryPoint.gameObject);
            Room roomB = RoomManager.GetRoomOfObject(door.roomBEntryPoint.gameObject);

            if(roomA == null)
            {
                Debug.LogError("Entry Point A was unable to assign Room. Please make sure the Room's floors are below the appropriate Door entry point.");
            }
            if(roomB == null)
            {
                Debug.LogError("Entry Point B was unable to assign Room. Please make sure the Room's floors are below the appropriate Door entry point.");
            }

            if (roomA && roomB)
            {
                // Link this door to found Rooms
                SerializedProperty roomAProperty = serializedObject.FindProperty("roomA");
                roomAProperty.objectReferenceValue = roomA;

                SerializedProperty roomBProperty = serializedObject.FindProperty("roomB");
                roomBProperty.objectReferenceValue = roomB;

                List<Door> roomADoors = new List<Door>(roomA.Doors);
                if(!roomADoors.Contains(door))
                {
                    roomADoors.Add(door);
                    roomA.Doors = roomADoors.ToArray();
                }

                List<Door> roomBDoors = new List<Door>(roomB.Doors);
                if(!roomBDoors.Contains(door))
                {
                    roomBDoors.Add(door);
                    roomB.Doors = roomADoors.ToArray();
                }
            }
        }
    }
}