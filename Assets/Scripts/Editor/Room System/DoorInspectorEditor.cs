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
        public void AutoLinkConnectingRooms()
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

                // Assign door
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

        [MenuItem("Room System/Auto-link all Doors")]
        private static void AutoLinkAllDoors()
        {
            ClearAllRoomDoorLinks();

            Door[] doors = FindObjectsOfType<Door>();

            foreach (var door in doors)
            {
                Room roomA = RoomManager.GetRoomOfObject(door.roomAEntryPoint.gameObject);
                Room roomB = RoomManager.GetRoomOfObject(door.roomBEntryPoint.gameObject);

                if (roomA == null)
                {
                    Debug.LogWarning($"{door.name}: Entry Point A was unable to assign to a Room. This may be intentional, if not, please make sure the Room's floors are below the appropriate Door entry point.", door);
                }
                if (roomB == null)
                {
                    Debug.LogWarning($"{door.name}: Entry Point B was unable to assign to a Room. This may be intentional, if not, please make sure the Room's floors are below the appropriate Door entry point.", door);
                }

                // Link this door to found Rooms
                SerializedObject doorObject = new SerializedObject(door);
                doorObject.Update();

                if (roomA != null) {
                    SerializedProperty roomAProperty = doorObject.FindProperty("roomA");
                    roomAProperty.objectReferenceValue = roomA;

                    // Add Door to Room
                    List<Door> roomADoors = new List<Door>(roomA.Doors);
                    if (!roomADoors.Contains(door))
                    {
                        roomADoors.Add(door);
                        roomA.Doors = roomADoors.ToArray();
                    }
                }

                if (roomB != null) {
                    SerializedProperty roomBProperty = doorObject.FindProperty("roomB");
                    roomBProperty.objectReferenceValue = roomB;

                    // Add Door to Room
                    List<Door> roomBDoors = new List<Door>(roomB.Doors);
                    if (!roomBDoors.Contains(door))
                    {
                        roomBDoors.Add(door);
                        roomB.Doors = roomBDoors.ToArray();
                    }
                }

                doorObject.ApplyModifiedProperties();
            }
        }

        [MenuItem("Room System/Clear all Room Door links")]
        private static void ClearAllRoomDoorLinks()
        {
            foreach (var room in FindObjectsOfType<Room>())
            {
                room.Doors = new Door[0];
            }
        }
    }
}