using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Utilities
{
    public class Spinner : MonoBehaviour
    {
        public float speed = 1.0f;
        public bool xAxis = false;
        public bool yAxis = false;
        public bool zAxis = false;

        private int  x = 0;
        private int  y = 0;
        private int  z = 0;

        private void Update() {
            x = xAxis ? 1 : 0;
            y = yAxis ? 1 : 0;
            z = zAxis ? 1 : 0;
            
            float tempSpeed = speed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(x * tempSpeed, y * tempSpeed, z * tempSpeed));
        }
    }
}
