using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement movement { get; private set; }

    private void Awake()
    {
        movement = GetComponentInChildren<Movement>();

        if (!movement)
        {
            Debug.LogError("Missing Core Component");
        }
    }
}
