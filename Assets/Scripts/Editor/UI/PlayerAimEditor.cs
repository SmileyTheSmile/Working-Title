using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerAim)), CanEditMultipleObjects]
class PlayerAimEditor : Editor
{
    private SerializedProperty _target;
    private SerializedProperty _cursor;
    private SerializedProperty _mousePositionInputSO;
    private SerializedProperty _clampMode;
    private SerializedProperty _clampThreshold;
    private SerializedProperty _clampRadius;

    private void OnEnable()
    {
        GetSerializedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawGUI();

        serializedObject.ApplyModifiedProperties();
    }

    private void GetSerializedProperties()
    {
        _target = serializedObject.FindProperty("_target");
        _cursor = serializedObject.FindProperty("_cursor");
        _mousePositionInputSO = serializedObject.FindProperty("_mousePositionInputSO");
        _clampMode = serializedObject.FindProperty("_clampMode");
        _clampThreshold = serializedObject.FindProperty("_clampThreshold");
        _clampRadius = serializedObject.FindProperty("_clampRadius");
    }

    private void DrawGUI()
    {
        EditorGUILayout.ObjectField(_target, new GUIContent("Camera Target", "The point at which the camera is looking."));
        EditorGUILayout.ObjectField(_cursor, new GUIContent("Cursor", "The object that will serve as a cursor"));
        EditorGUILayout.ObjectField(_mousePositionInputSO, new GUIContent("Mouse Pos SO", "The scriptable object of the mouse's current position."));

        EditorGUILayout.PropertyField(_clampMode, new GUIContent("Clamp Mode", "The way in which the camera target is clamped."));

        if (_clampMode.intValue == (int)ClampModes.rectangle)
        {
            EditorGUILayout.PropertyField(_clampThreshold, new GUIContent("Clamp Threshold", "Size of the rectangular border of the target."));
        }
        else if (_clampMode.intValue == (int)ClampModes.circle)
        {
            EditorGUILayout.PropertyField(_clampRadius, new GUIContent("Clamp Radius", "Radius of the circular border of the target."));
        }
    }
}