using UnityEngine;
using UnityEditor;

public abstract class GenericEditor : Editor
{
    public virtual void OnEnable()
    {
        GetSerializedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawGUI();
        serializedObject.ApplyModifiedProperties();
    }

    public virtual void GetSerializedProperties() { }
    public virtual void DrawGUI() { }
}
