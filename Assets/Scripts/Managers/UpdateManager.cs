using UnityEngine;
using Events;

public class UpdateManager : MonoBehaviour
{
    [SerializeField] private ScriptableEvent _updateEvent;

    [SerializeField] private ScriptableEvent _fixedEvent;

    private void Update()
    {
        _updateEvent.Dispatch();
    }

    private void FixedUpdate()
    {
        _fixedEvent.Dispatch();
    }
}