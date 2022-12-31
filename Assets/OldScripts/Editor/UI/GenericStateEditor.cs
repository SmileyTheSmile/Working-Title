using UnityEditor;

[CustomEditor(typeof(GenericState), true)]
class CustomStateEditor : Editor
{
    protected SerializedProperty _transitions;
    protected SerializedProperty _animBoolName;

    protected virtual void OnEnable()
    {
        GetSerializedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawGUI();

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void GetSerializedProperties()
    {
        _transitions = serializedObject.FindProperty("_transitions");
        _animBoolName = serializedObject.FindProperty("_animBoolName");
    }

    protected virtual void DrawGUI()
    {
        //EditorGUILayout.PropertyField(_animBoolName, new GUIContent("Animation Bool", "Name of the state's animator bool."));

        DrawTransitions();
    }

    protected virtual void DrawTransitions()
    {
        //EditorGUILayout.PropertyField(_transitions, new GUIContent("Transitions", "The list of states this state can transition to."));

        EditorGUI.indentLevel += 1;

        if (_transitions.isExpanded)
        {
            for (int i = 0; i < _transitions.arraySize; i++)
            {
                //EditorGUILayout.PropertyField(_transitions.GetArrayElementAtIndex(i));
            }
        }

        EditorGUI.indentLevel -= 1;
    }
}

[CustomEditor(typeof(PlayerState), true)]
class CustomPlayerStateEditor : CustomStateEditor
{
    protected SerializedProperty _playerData;

    protected override void GetSerializedProperties()
    {
        base.GetSerializedProperties();

        _playerData = serializedObject.FindProperty("_playerData");
    }

    protected override void DrawGUI()
    {
        //EditorGUILayout.ObjectField(_playerData, new GUIContent("Player Data", "Stuff that's used in the classes."));

        DrawDefaultInspector();

        base.DrawGUI();
    }
}