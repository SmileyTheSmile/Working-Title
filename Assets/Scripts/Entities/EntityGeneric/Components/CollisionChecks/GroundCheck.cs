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
        condition.value = Physics2D.OverlapBox(transform.position, new Vector2(_groundCheckWidth, _groundCheckHeight), 0f, _whatIsGround);
    }

    //Draw gizmos
    void OnDrawGizmos()
    {
        if (condition.value)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.blue;

        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x - _halfGroundCheckWidth, transform.position.y + _halfGroundCheckHeight), new Vector2(transform.position.x + _halfGroundCheckWidth, transform.position.y + _halfGroundCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x - _halfGroundCheckWidth, transform.position.y + _halfGroundCheckHeight), new Vector2(transform.position.x - _halfGroundCheckWidth, transform.position.y - _halfGroundCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x + _halfGroundCheckWidth, transform.position.y - _halfGroundCheckHeight), new Vector2(transform.position.x + _halfGroundCheckWidth, transform.position.y + _halfGroundCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x + _halfGroundCheckWidth, transform.position.y - _halfGroundCheckHeight), new Vector2(transform.position.x - _halfGroundCheckWidth, transform.position.y - _halfGroundCheckHeight));
    }
}
