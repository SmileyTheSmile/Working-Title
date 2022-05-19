using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Transform _playerOriginPoint;
    [SerializeField] private Transform _target;
    [SerializeField] private ScriptableVector3 _mousePositionInputSO;

    [SerializeField] private ClampModes _clampMode = ClampModes.circle;
    [SerializeField] private Vector2 _clampThreshold = new Vector2(2f, 2f);
    [SerializeField] private float _clampRadius = 2.5f;

    private Camera _mainCamera;
    private Vector3 _mousePosition => _mousePositionInputSO.value;

    private void Awake()
    {
        _mainCamera = Camera.main;

        _target = transform.Find("CameraTarget");
    }

    private void Update()
    {
        UpdateCursorPosition();
        UpdateCameraTargetPosition();
    }

    private void UpdateCursorPosition()
    {
        transform.position = _mousePosition;
    }

    private void UpdateCameraTargetPosition()
    {
        switch (_clampMode)
        {
            case ClampModes.rectangle:
                ClampTargetInRect();
                break;
            case ClampModes.circle:
                ClampTargetInCircle();
                break;
        }
    }

    private void ClampTargetInRect()
    {
        Vector3 targetPosition = (_playerOriginPoint.position + _mousePosition) / 2;

        targetPosition.x = Mathf.Clamp(targetPosition.x, _playerOriginPoint.position.x - _clampThreshold.x, _playerOriginPoint.position.x + _clampThreshold.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, _playerOriginPoint.position.y - _clampThreshold.y, _playerOriginPoint.position.y + _clampThreshold.y);

        _target.position = targetPosition;
    }

    private void ClampTargetInCircle()
    {
        Vector3 targetPosition = (_mousePosition - _playerOriginPoint.position) / 2;

        targetPosition = Vector3.ClampMagnitude(targetPosition, _clampRadius);

        _target.position = _playerOriginPoint.position + targetPosition;
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawDottedLine(_playerOriginPoint.position, transform.position, 1f);

        UnityEditor.Handles.DrawWireDisc(_target.position, Vector3.forward, 0.2f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, 0.2f);
        UnityEditor.Handles.DrawWireDisc(_playerOriginPoint.position, Vector3.forward, 0.4f);

        if (_clampMode == ClampModes.rectangle)
        {
            UnityEditor.Handles.DrawDottedLine(_target.position, transform.position, 1f);
            UnityEditor.Handles.DrawDottedLine(_playerOriginPoint.position, _target.position, 1f);

            UnityEditor.Handles.DrawLine(new Vector2(_playerOriginPoint.position.x - _clampThreshold.x, _playerOriginPoint.position.y + _clampThreshold.y), new Vector2(_playerOriginPoint.position.x + _clampThreshold.x, _playerOriginPoint.position.y + _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerOriginPoint.position.x - _clampThreshold.x, _playerOriginPoint.position.y + _clampThreshold.y), new Vector2(_playerOriginPoint.position.x - _clampThreshold.x, _playerOriginPoint.position.y - _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerOriginPoint.position.x + _clampThreshold.x, _playerOriginPoint.position.y + _clampThreshold.y), new Vector2(_playerOriginPoint.position.x + _clampThreshold.x, _playerOriginPoint.position.y - _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerOriginPoint.position.x + _clampThreshold.x, _playerOriginPoint.position.y - _clampThreshold.y), new Vector2(_playerOriginPoint.position.x - _clampThreshold.x, _playerOriginPoint.position.y - _clampThreshold.y));
        }
        else if (_clampMode == ClampModes.circle)
        {
            UnityEditor.Handles.DrawWireDisc(_playerOriginPoint.position, Vector3.forward, _clampRadius);
        }
    }
}

public enum ClampModes
{
    rectangle,
    circle
}

#if UNITY_EDITOR

[CustomEditor(typeof(CustomCursor)), CanEditMultipleObjects]
class CustomCursorEditor : Editor
{
    private SerializedProperty _playerOriginPoint;
    private SerializedProperty _target;
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
        _playerOriginPoint = serializedObject.FindProperty("_playerOriginPoint");
        _target = serializedObject.FindProperty("_target");
        _mousePositionInputSO = serializedObject.FindProperty("_mousePositionInputSO");
        _clampMode = serializedObject.FindProperty("_clampMode");
        _clampThreshold = serializedObject.FindProperty("_clampThreshold");
        _clampRadius = serializedObject.FindProperty("_clampRadius");
    }

    private void DrawGUI()
    {
        EditorGUILayout.ObjectField(_playerOriginPoint, new GUIContent("Player Origin Point", "The center around which the cursor can move."));
        EditorGUILayout.ObjectField(_target, new GUIContent("Camera Target", "The point at which the camera is looking."));
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

#endif