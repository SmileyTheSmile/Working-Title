using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Check if entity is standing on a ledge
public class LedgeVerticalCheck : CollisionCheck
{
    [SerializeField] private ScriptableInt _movementDirection;
    [SerializeField] private float _ledgeCheckDistance = 0.6f;

    protected override void Update()
    {
        condition.value = Physics2D.Raycast(transform.position, Vector2.right * _movementDirection.value, _ledgeCheckDistance, _whatIsGround);
    }
}
