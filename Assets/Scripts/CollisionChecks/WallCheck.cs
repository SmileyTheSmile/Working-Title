using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : CollisionCheck
{
    [SerializeField] private WallCheckType _wallChecktype = WallCheckType.front;
    [SerializeField] private ScriptableInt _movementDirection;
    [SerializeField] private float _wallCheckDistance = 0.6f;

    protected override void Update()
    {
        condition.value = Physics2D.Raycast(transform.position, Vector2.right * (int)_wallChecktype * _movementDirection.value, _wallCheckDistance, _whatIsGround);
    }

    //Draw gizmos
    void OnDrawGizmos()
    {
        if (condition.value)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.blue;

        UnityEditor.Handles.DrawLine(transform.position, new Vector2(transform.position.x + (int)_wallChecktype * _movementDirection.value * _wallCheckDistance, transform.position.y));
    }

    private enum WallCheckType
    {
        front = 1,
        back = -1
    }
}
