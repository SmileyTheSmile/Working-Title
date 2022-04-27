using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement movement
    {
        get => GenericNotImplementedError<Movement>.TryGet(_movement, transform.parent.name);
        private set => _movement = value;
    }

    public CollisionSenses collisionSenses
    {
        get => GenericNotImplementedError<CollisionSenses>.TryGet(_collisionSenses, transform.parent.name);
        private set => _collisionSenses = value;
    }

    public WeaponHandler weaponHandler
    {
        get => GenericNotImplementedError<WeaponHandler>.TryGet(_weaponHandler, transform.parent.name);
        private set => _weaponHandler = value;
    }

    public Combat combat
    {
        get => GenericNotImplementedError<Combat>.TryGet(_combat, transform.parent.name);
        private set => _combat = value;
    }

    private Movement _movement;
    private CollisionSenses _collisionSenses;
    private WeaponHandler _weaponHandler;
    private Combat _combat;

    public void Awake()
    {
        _movement = GetComponentInChildren<Movement>();
        _collisionSenses = GetComponentInChildren<CollisionSenses>();
        _weaponHandler = GetComponentInChildren<WeaponHandler>();
        _combat = GetComponentInChildren<Combat>();
    }

    public void LogicUpdate()
    {
        _movement.LogicUpdate();
    }
}