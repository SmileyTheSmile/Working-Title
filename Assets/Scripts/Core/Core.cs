using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement movement { get; private set; }
    public CollisionSenses collisionSenses { get; private set; }

    private void Awake()
    {
        movement = GetComponentInChildren<Movement>();
        collisionSenses = GetComponentInChildren<CollisionSenses>();

        if (!movement)
        {
            Debug.LogError("Missing Movement Core Component");
        }
        if (!collisionSenses)
        {
            Debug.LogError("Missing Collision Senses Core Component");
        }
    }

    public void LogicUpdate()
    {
        movement.LogicUpdate();
    }
}
