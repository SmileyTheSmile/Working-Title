using UnityEngine;

/// <summary>
/// Check if entity is standing on a ledge
/// </summary>
public class LedgeVerticalCheck : CollisionCheck
{
    /*
    [SerializeField] private ScriptableInt _movementDirection;
    [SerializeField] private float _ledgeCheckDistance = 0.6f;

    protected override void Update()
    {
        _conditions.IsTouchingLedgeVertical = Physics2D.Raycast(transform.position, Vector2.right * _movementDirection.value, _ledgeCheckDistance, _whatIsGround);
    }
    */
}
