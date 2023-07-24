using UnityEngine;

/// <summary>
/// Check if entity is grounded
/// </summary>
public class GroundCheck : CollisionCheck
{
    [SerializeField] private float _groundCheckHeight = 0.2f;
    [SerializeField] private float _groundWidthOffset = 0.05f;
    [SerializeField] private Vector2 defaultColliderSize = new Vector2(1, 1);

    private float _groundCheckWidth => defaultColliderSize.x - _groundWidthOffset;
    private float _halfGroundCheckWidth => _groundCheckWidth / 2;
    private float _halfGroundCheckHeight => _groundCheckHeight / 2;

    protected override void Update()
    {
        _conditions.IsGrounded = Physics2D.OverlapBox(transform.position, new Vector2(_groundCheckWidth, _groundCheckHeight), 0f, _whatIsGround);
    }

#if UNITY_EDITOR
    private Vector2 topLeft;
    private Vector2 topRight;
    private Vector2 bottomLeft;
    private Vector2 bottomRight;

    void OnDrawGizmos()
    {
        if (_conditions.IsGrounded)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.blue;

        topLeft = new Vector2(transform.position.x - _halfGroundCheckWidth, transform.position.y + _halfGroundCheckHeight);
        topRight = new Vector2(transform.position.x + _halfGroundCheckWidth, transform.position.y + _halfGroundCheckHeight);
        bottomLeft = new Vector2(transform.position.x - _halfGroundCheckWidth, transform.position.y - _halfGroundCheckHeight);
        bottomRight = new Vector2(transform.position.x + _halfGroundCheckWidth, transform.position.y - _halfGroundCheckHeight);

        UnityEditor.Handles.DrawLine(topLeft, topRight);
        UnityEditor.Handles.DrawLine(topLeft, bottomLeft);
        UnityEditor.Handles.DrawLine(bottomRight, topRight);
        UnityEditor.Handles.DrawLine(bottomRight, bottomLeft);
    }
#endif
}
