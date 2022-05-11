using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        condition.value = Physics2D.OverlapBox(transform.position, new Vector2(_ceilingCheckWidth, _ceilingCheckHeight), 0f, _whatIsGround);
    }

    //Draw gizmos
    void OnDrawGizmos()
    {
        if (condition.value)
            UnityEditor.Handles.color = Color.green;
        else
            UnityEditor.Handles.color = Color.blue;

        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x - _halfCeilingCheckWidth, transform.position.y + _halfCeilingCheckHeight), new Vector2(transform.position.x + _halfCeilingCheckWidth, transform.position.y + _halfCeilingCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x - _halfCeilingCheckWidth, transform.position.y + _halfCeilingCheckHeight), new Vector2(transform.position.x - _halfCeilingCheckWidth, transform.position.y - _halfCeilingCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x + _halfCeilingCheckWidth, transform.position.y - _halfCeilingCheckHeight), new Vector2(transform.position.x + _halfCeilingCheckWidth, transform.position.y + _halfCeilingCheckHeight));
        UnityEditor.Handles.DrawLine(new Vector2(transform.position.x + _halfCeilingCheckWidth, transform.position.y - _halfCeilingCheckHeight), new Vector2(transform.position.x - _halfCeilingCheckWidth, transform.position.y - _halfCeilingCheckHeight));
    }
}
