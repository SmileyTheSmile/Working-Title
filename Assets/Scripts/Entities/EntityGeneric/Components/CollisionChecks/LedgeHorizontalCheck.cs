using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeHorizontalCheck : CollisionCheck
{
    [SerializeField] private ScriptableInt _movementDirection;
    [SerializeField] private float _ledgeCheckDistance = 0.6f;

    protected override void Update()
    {
        condition.value = Physics2D.Raycast(transform.position, Vector2.right * _movementDirection.value, _ledgeCheckDistance, _whatIsGround);
    }

    //Draw gizmos
    void OnDrawGizmos()
    {
        if (condition.value)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.blue;

        UnityEditor.Handles.DrawLine(transform.position, new Vector2(transform.position.x + _ledgeCheckDistance * _movementDirection.value, transform.position.y));
    }
}