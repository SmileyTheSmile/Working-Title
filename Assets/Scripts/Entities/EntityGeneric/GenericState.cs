using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class GenericState : ScriptableObject
{
    [SerializeField] protected List<StateTransition> _transitions = new List<StateTransition>();
    [SerializeField] protected string _animBoolName;

    protected EntityGeneric _entity;
    protected float _startTime;
    protected bool _isAnimationFinished;
    protected bool _isExitingState;

    //What to do when entering the state
    public virtual void Enter() 
    {
        _startTime = Time.time;
        _isExitingState = false;
        _isAnimationFinished = false;
    }

    //What to do when exiting the state
    public virtual void Exit()
    {
        _isExitingState = true;
    }
    
    public virtual void SetCore(EntityGeneric entity)
    {
        _entity = entity;
    }
    
    //Do all the checks if the state should transition into another state
    public virtual void DoActions() { if (_isExitingState) return; }
    //Execute all the actions the state must do every Update
    public abstract GenericState DoTransitions();
    //What to do in animation events in Animator
    public virtual void AnimationTrigger() { }
    //What to do on finished animation in Animator
    public virtual void AnimationFinishedTrigger() => _isAnimationFinished = true; 
}

#if UNITY_EDITOR

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
        EditorGUILayout.PropertyField(_animBoolName, new GUIContent("Animation Bool", "Name of the state's animator bool."));

        DrawTransitions();
    }

    protected virtual void DrawTransitions()
    {
        EditorGUILayout.PropertyField(_transitions, new GUIContent("Transitions", "The list of states this state can transition to."));

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
        EditorGUILayout.ObjectField(_playerData, new GUIContent("Player Data", "Stuff that's used in the classes."));

        base.DrawGUI();
    }
}

#endif