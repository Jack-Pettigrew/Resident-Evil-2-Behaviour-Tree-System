using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

public class GetRoomTest : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Debug.Log(RoomManager.GetRoomOfObject(transform.gameObject));
    }
}
