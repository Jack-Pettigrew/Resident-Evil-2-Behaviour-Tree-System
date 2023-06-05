using System;
using UnityEngine;

namespace DD.AI.Sensors
{
    public class AIHear : MonoBehaviour
    {
        public event Action OnSoundHeard;
        
        public void HearSound()
        {
            OnSoundHeard?.Invoke();
        }
    }
}