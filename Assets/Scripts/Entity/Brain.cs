using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

namespace Platformer.Entity
{
    public class Brain : MonoBehaviour
    {
        [SerializeField] private EventListener _updateListener;
        [SerializeField] private EventListener _fixedUpdateListener;

        private void OnEnable()
        {
            _updateListener.OnEventHappened += EventUpdate;
            _fixedUpdateListener.OnEventHappened += EventFixedUpdate;
        }

        private void EventUpdate()
        {
            Debug.Log("Updated brain");
        }

        private void EventFixedUpdate()
        {
            Debug.Log("Fixed updated brain");
        }
    }
}
