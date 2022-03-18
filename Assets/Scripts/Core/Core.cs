using UnityEngine;

public class Core : MonoBehaviour
{
    #region Public Wrapper Core Components

    public Movement movement
    {
        get
        {
            if (_movement)
            {
                return _movement;
            }

            Debug.Log("Missing Movement Core Component on " + transform.parent.name);

            return null;
        }
        private set => _movement = value;
    }

    public CollisionSenses collisionSenses
    {
        get
        {
            if (_collisionSenses)
            {
                return _collisionSenses;
            }

            Debug.Log("Missing Collision Senses Core Component on " + transform.parent.name);

            return null;
        }
        private set => _collisionSenses = value;
    }

    #endregion

    #region Private Core Components

    private Movement _movement;

    private CollisionSenses _collisionSenses;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        _movement = GetComponentInChildren<Movement>();
        _collisionSenses = GetComponentInChildren<CollisionSenses>();
    }

    #endregion

    #region Update Functions

    public void LogicUpdate()
    {
        _movement.LogicUpdate();
    }

    #endregion
}
