using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Transform _playerOrigin;
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
        Vector3 targetPosition = (_playerOrigin.position + _mousePosition) / 2;

        targetPosition.x = Mathf.Clamp(targetPosition.x, _playerOrigin.position.x - _clampThreshold.x, _playerOrigin.position.x + _clampThreshold.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, _playerOrigin.position.y - _clampThreshold.y, _playerOrigin.position.y + _clampThreshold.y);

        _target.position = targetPosition;
    }

    private void ClampTargetInCircle()
    {
        Vector3 targetPosition = (_mousePosition - _playerOrigin.position) / 2;

        targetPosition = Vector3.ClampMagnitude(targetPosition, _clampRadius);

        _target.position = _playerOrigin.position + targetPosition;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawDottedLine(_playerOrigin.position, transform.position, 1f);

        UnityEditor.Handles.DrawWireDisc(_target.position, Vector3.forward, 0.2f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, 0.2f);
        UnityEditor.Handles.DrawWireDisc(_playerOrigin.position, Vector3.forward, 0.4f);

        if (_clampMode == ClampModes.rectangle)
        {
            UnityEditor.Handles.DrawDottedLine(_target.position, transform.position, 1f);
            UnityEditor.Handles.DrawDottedLine(_playerOrigin.position, _target.position, 1f);

            UnityEditor.Handles.DrawLine(new Vector2(_playerOrigin.position.x - _clampThreshold.x, _playerOrigin.position.y + _clampThreshold.y), new Vector2(_playerOrigin.position.x + _clampThreshold.x, _playerOrigin.position.y + _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerOrigin.position.x - _clampThreshold.x, _playerOrigin.position.y + _clampThreshold.y), new Vector2(_playerOrigin.position.x - _clampThreshold.x, _playerOrigin.position.y - _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerOrigin.position.x + _clampThreshold.x, _playerOrigin.position.y + _clampThreshold.y), new Vector2(_playerOrigin.position.x + _clampThreshold.x, _playerOrigin.position.y - _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(_playerOrigin.position.x + _clampThreshold.x, _playerOrigin.position.y - _clampThreshold.y), new Vector2(_playerOrigin.position.x - _clampThreshold.x, _playerOrigin.position.y - _clampThreshold.y));
        }
        else if (_clampMode == ClampModes.circle)
        {
            UnityEditor.Handles.DrawWireDisc(_playerOrigin.position, Vector3.forward, _clampRadius);
        }
    }
#endif
}

public enum ClampModes
{
    rectangle,
    circle
}
