using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.Sensors
{
    public abstract class AISensor : MonoBehaviour
    {
        /// <summary>
        /// Senses 
        /// </summary>
        /// <returns>Whether the sensor has sensed something.</returns>
        public abstract bool Sense();
    }
}
