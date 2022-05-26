using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Try making those into ScriptableObjects
public class CollisionCheck : MonoBehaviour
{
    public CollisionCheckTransitionCondition condition;
    
    [SerializeField] protected LayerMask _whatIsGround;

    protected virtual void Update() { }
}
