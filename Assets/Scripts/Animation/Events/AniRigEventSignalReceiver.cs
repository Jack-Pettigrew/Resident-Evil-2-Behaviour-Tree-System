using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.Animation.RigEvents
{
    public class AniRigEventSignalReceiver : MonoBehaviour
    {
        [SerializeField] private string receiverLabel;
        public int ReceiverHash { private set; get; }

        public UnityEvent<GameObject> OnTrigger;
        public UnityEvent<GameObject> OnEnable;
        public UnityEvent<GameObject> OnDisable;
        public UnityEvent<GameObject> OnStart;
        public UnityEvent<GameObject> OnStop;

        private void Awake()
        {
            ReceiverHash = receiverLabel.GetHashCode();
        }

        public void ReceiveSignal(AnimRigEventType eventType)
        {            
            switch (eventType)
            {
                case AnimRigEventType.START:
                    OnStart?.Invoke(null);
                    break;

                case AnimRigEventType.STOP:
                    OnDisable?.Invoke(null);
                    break;

                case AnimRigEventType.TRIGGER:
                    OnTrigger?.Invoke(null);
                    break;

                case AnimRigEventType.ENABLE:
                    OnEnable?.Invoke(null);
                    break;

                case AnimRigEventType.DISABLE:
                    OnDisable?.Invoke(null);
                    break;

                default:
                    Debug.LogError($"{name} could not handle AnimRigEvent {eventType.ToString()}.", this);
                    break;
            }
        }
    }

    public enum AnimRigEventType
    {
        START,
        STOP,
        TRIGGER,
        ENABLE,
        DISABLE
    }
}
