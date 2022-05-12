using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Cursor : MonoBehaviour
{
    [SerializeField] private Transform _playerCenter;
    [SerializeField] private Transform _target;
    [SerializeField] private ScriptableVector3 _mousePositionInputSO;
    
    [SerializeField] private ClampModes _clampMode = ClampModes.circle;
    [SerializeField] private Vector2 _clampThreshold = new Vector2(2f, 2f);
    [SerializeField] private float _clampRadius = 2.5f;

    private Camera _mainCamera;
    private Vector3 _targetPosition;
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
        _targetPosition = (_playerCenter.position + _mousePosition) / 2;

        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _playerCenter.position.x - _clampThreshold.x, _playerCenter.position.x + _clampThreshold.x);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y, _playerCenter.position.y - _clampThreshold.y, _playerCenter.position.y + _clampThreshold.y);

        _target.position = _targetPosition;
    }

    private void ClampTargetInCircle()
    {
        _targetPosition = (_mousePosition - _playerCenter.position) / 2;

        _targetPosition = Vector3.ClampMagnitude(_targetPosition, _clampRadius);

        _target.position = _playerCenter.position + _targetPosition;
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawDottedLine(_playerCenter.position, transform.position, 1f);
        UnityEditor.Handles.DrawDottedLine(_target.position, transform.position, 1f);
        UnityEditor.Handles.DrawDottedLine(_playerCenter.position, _target.position, 1f);

        UnityEditor.Handles.DrawWireDisc(_target.position, Vector3.forward, 0.2f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, 0.2f);

        if (_clampMode == ClampModes.rectangle)
        {
            UnityEditor.Handles.DrawLine(new Vector2(_playerCenter.position.x - _clampThreshold.x, _playerCenter.position.y + _clampThreshold.y), new Vector2(_playerCenter.position.x + _clampThreshold.x, _playerCenter.position.y + _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerCenter.position.x - _clampThreshold.x, _playerCenter.position.y + _clampThreshold.y), new Vector2(_playerCenter.position.x - _clampThreshold.x, _playerCenter.position.y - _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerCenter.position.x + _clampThreshold.x, _playerCenter.position.y + _clampThreshold.y), new Vector2(_playerCenter.position.x + _clampThreshold.x, _playerCenter.position.y - _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerCenter.position.x + _clampThreshold.x, _playerCenter.position.y - _clampThreshold.y), new Vector2(_playerCenter.position.x - _clampThreshold.x, _playerCenter.position.y - _clampThreshold.y));
        }
        else if (_clampMode == ClampModes.circle)
        {
            UnityEditor.Handles.DrawWireDisc(_playerCenter.position, Vector3.forward, _clampRadius);
        }
    }

    private enum ClampModes
    {
        rectangle,
        circle
    }

#if UNITY_EDITOR
    //[CustomEditor(typeof(Cursor))]
    class CursorEditor : Editor
    {
        private Cursor _cursor;

        private void OnEnable()
        {
            _cursor = (Cursor)target;
            if (_cursor == null) return;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _cursor._playerCenter = (Transform)EditorGUILayout.ObjectField("Player Center", _cursor._playerCenter, typeof(Transform), true);
            _cursor._target = (Transform)EditorGUILayout.ObjectField("Target", _cursor._target, typeof(Transform), true);

            _cursor._clampMode = (ClampModes)EditorGUILayout.EnumPopup("Clamp Mode", _cursor._clampMode);

            if (_cursor._clampMode == ClampModes.rectangle)
            {
                _cursor._clampThreshold = EditorGUILayout.Vector2Field("Clamp Threshold", _cursor._clampThreshold);
            }
            else if (_cursor._clampMode == ClampModes.circle)
            {
                _cursor._clampRadius = EditorGUILayout.FloatField("Clamp Radius", _cursor._clampRadius);
            }

            EditorUtility.SetDirty(target);
        }
    }
#endif
}
