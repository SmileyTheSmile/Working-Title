using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check if entity is nearing a ledge when wall climbing
/// </summary>
public class LedgeHorizontalCheck : CollisionCheck
{
    [SerializeField] private float _ledgeCheckDistance = 0.6f;

    protected override void Update()
    {
        _conditions.IsTouchingLedgeHorizontal = Physics2D.Raycast(transform.position, Vector2.right * _conditions.MovementDir, _ledgeCheckDistance, _whatIsGround);
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (_conditions.IsTouchingLedgeHorizontal)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.blue;

        UnityEditor.Handles.DrawLine(transform.position, new Vector2(transform.position.x + _ledgeCheckDistance * _conditions.MovementDir, transform.position.y));
    }
#endif
}
