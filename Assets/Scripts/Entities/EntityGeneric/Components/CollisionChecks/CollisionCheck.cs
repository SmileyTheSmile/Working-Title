using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    [SerializeField] protected PlayerStats _conditions;
    
    [SerializeField] protected LayerMask _whatIsGround;

    protected virtual void Update() { }
}
