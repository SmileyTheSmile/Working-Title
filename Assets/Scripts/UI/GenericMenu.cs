using UnityEngine;
using UnityEngine.UI;

public abstract class GenericMenu : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        AddAllListeners();
    }

    protected virtual void OnDisable()
    {
        RemoveAllListeners();
    }

    protected abstract void RemoveAllListeners();
    protected abstract void AddAllListeners();
}
