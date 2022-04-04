using UnityEngine;

public class Core : MonoBehaviour
{
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

    public WeaponHandler weaponHandler
    {
        get
        {
            if (_collisionSenses)
            {
                return _weaponHandler;
            }

            Debug.Log("Missing Weapons Core Component on " + transform.parent.name);

            return null;
        }
        private set => _weaponHandler = value;
    }

    protected Movement _movement;
    protected CollisionSenses _collisionSenses;
    protected WeaponHandler _weaponHandler;

    public void Awake()
    {
        _movement = GetComponentInChildren<Movement>();
        _collisionSenses = GetComponentInChildren<CollisionSenses>();
        _weaponHandler = GetComponentInChildren<WeaponHandler>();
    }

    public void LogicUpdate()
    {
        _movement.LogicUpdate();
    }

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
}