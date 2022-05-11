using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public StateCollisionCheckCondition condition;
    
    [SerializeField] protected LayerMask _whatIsGround;

    protected virtual void Update() { }
}
