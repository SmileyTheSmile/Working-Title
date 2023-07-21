using UnityEngine;

/// <summary>
/// Check if entity is touching ceiling
/// </summary>
public class CeilingCheck : CollisionCheck
{
    [SerializeField] private float _ceilingCheckHeight = 0.2f;
    [SerializeField] private float _ceilingWidthOffset = 0.05f;
    [SerializeField] private Vector2 defaultColliderSize = new Vector2(1, 1);

    private float _ceilingCheckWidth => defaultColliderSize.x - _ceilingWidthOffset;
    private float _halfCeilingCheckWidth => _ceilingCheckWidth / 2;
    private float _halfCeilingCheckHeight => _ceilingCheckHeight / 2;

    protected override void Update()
    {
        _conditions.IsTouchingCeiling = Physics2D.OverlapBox(transform.position, new Vector2(_ceilingCheckWidth, _ceilingCheckHeight), 0f, _whatIsGround);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (_conditions.IsTouchingCeiling)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.blue;

        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x - _halfCeilingCheckWidth, transform.position.y + _halfCeilingCheckHeight), new Vector2(transform.position.x + _halfCeilingCheckWidth, transform.position.y + _halfCeilingCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x - _halfCeilingCheckWidth, transform.position.y + _halfCeilingCheckHeight), new Vector2(transform.position.x - _halfCeilingCheckWidth, transform.position.y - _halfCeilingCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x + _halfCeilingCheckWidth, transform.position.y - _halfCeilingCheckHeight), new Vector2(transform.position.x + _halfCeilingCheckWidth, transform.position.y + _halfCeilingCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x + _halfCeilingCheckWidth, transform.position.y - _halfCeilingCheckHeight), new Vector2(transform.position.x - _halfCeilingCheckWidth, transform.position.y - _halfCeilingCheckHeight));
    }
#endif
}
