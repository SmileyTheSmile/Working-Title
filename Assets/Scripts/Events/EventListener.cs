using System;
using UnityEngine;

namespace Events
{
    [Serializable]
    public class EventListener
    {
        [SerializeField] private ScriptableEvent _event;

        public event Action OnEventHappened = delegate { };

        public void Enable()
        {
            _event.AddListener(EventHappened);
        }

        public void Disable()
        {
            _event.RemoveListener(EventHappened);
        }

        private void EventHappened()
        {
            OnEventHappened.Invoke();
        }
    }
}