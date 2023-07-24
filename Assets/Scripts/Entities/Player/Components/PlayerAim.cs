using UnityEngine;

public class PlayerAim : CoreComponent
{
    [SerializeField] private Transform _cursor;
    [SerializeField] private Transform _target;
    [SerializeField] protected PlayerConditionTable _conditions;

    [SerializeField] private ClampModes _clampMode = ClampModes.circle;
    [SerializeField] private Vector2 _clampThreshold = new Vector2(2f, 2f);
    [SerializeField] private float _clampRadius = 2.5f;

    public override void LogicUpdate()
    {
        UpdateCursorPosition();
        UpdateCameraTargetPosition();
    }

    private void UpdateCursorPosition()
    {
        _cursor.position = _conditions.MousePosition;
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
        Vector3 newTargetPosition = (base.transform.position + _conditions.MousePosition) / 2;

        newTargetPosition.x = Mathf.Clamp(newTargetPosition.x, transform.position.x - _clampThreshold.x, transform.position.x + _clampThreshold.x);
        newTargetPosition.y = Mathf.Clamp(newTargetPosition.y, transform.position.y - _clampThreshold.y, transform.position.y + _clampThreshold.y);

        _target.position = newTargetPosition;
    }

    private void ClampTargetInCircle()
    {
        Vector3 newTargetPosition = (_conditions.MousePosition - transform.position) / 2;
        newTargetPosition = Vector3.ClampMagnitude(newTargetPosition, _clampRadius);
        _target.position = transform.position + newTargetPosition;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawDottedLine(transform.position, _cursor.position, 1f);

        UnityEditor.Handles.DrawWireDisc(_target.position, Vector3.forward, 0.2f);
        UnityEditor.Handles.DrawWireDisc(_cursor.position, Vector3.forward, 0.2f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, 0.4f);

        if (_clampMode == ClampModes.rectangle)
        {
            UnityEditor.Handles.DrawDottedLine(_target.position, _cursor.position, 1f);
            UnityEditor.Handles.DrawDottedLine(transform.position, _target.position, 1f);

            UnityEditor.Handles.DrawLine(new Vector2(transform.position.x - _clampThreshold.x, transform.position.y + _clampThreshold.y), new Vector2(transform.position.x + _clampThreshold.x, transform.position.y + _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(transform.position.x - _clampThreshold.x, transform.position.y + _clampThreshold.y), new Vector2(transform.position.x - _clampThreshold.x, transform.position.y - _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(transform.position.x + _clampThreshold.x, transform.position.y + _clampThreshold.y), new Vector2(transform.position.x + _clampThreshold.x, transform.position.y - _clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(transform.position.x + _clampThreshold.x, transform.position.y - _clampThreshold.y), new Vector2(transform.position.x - _clampThreshold.x, transform.position.y - _clampThreshold.y));
        }
        else if (_clampMode == ClampModes.circle)
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _clampRadius);
        }
    }
#endif
}

public enum ClampModes
{
    rectangle,
    circle
}
