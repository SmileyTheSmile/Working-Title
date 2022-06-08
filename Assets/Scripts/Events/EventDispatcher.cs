using System;
using UnityEngine;

namespace Events
{
    [Serializable]
    public class EventDispatcher
    {
        [SerializeField] private ScriptableEvent _someEvent;

        public void Dispatch()
        {
            _someEvent.Dispatch();
        }
    }
}