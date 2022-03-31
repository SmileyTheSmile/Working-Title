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

    protected Movement _movement;
    protected CollisionSenses _collisionSenses;

    #endregion

    #region Unity Functions

    public void Awake()
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

    #region Utility Functions

    public void SquashColliderDown(float biggerHeight, float smallerHeight)
    {
        _movement.SquashColliderDown(smallerHeight);

        if (_collisionSenses.ceilingCheck)
        {
            _collisionSenses.ceilingCheck.transform.position -= Vector3.up * ((biggerHeight - smallerHeight) * _movement.defaultSize.y);
        }
    }

    public void UnSquashColliderDown(float biggerHeight, float smallerHeight)
    {
        movement.ResetColliderHeight();

        if (_collisionSenses.ceilingCheck)
        {
            _collisionSenses.ceilingCheck.transform.position += Vector3.up * ((biggerHeight - smallerHeight) * _movement.defaultSize.y);
        }
    }

    #endregion
}