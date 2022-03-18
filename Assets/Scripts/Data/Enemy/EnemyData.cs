using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]

public class EnemyData : ScriptableObject
{
    #region Idle State

    [Header("Idle State")]
    public float minIdleTime = 1f;
    public float maxIdleTime = 2f;

    #endregion

    #region Move State

    [Header("Move State")]
    public float movementVelocity = 3f;

    #endregion
}
