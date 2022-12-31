using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Movement movement
    { get => _movement ?? _player.GetCoreComponent(ref _movement); }
    private Movement _movement;
    protected TemporaryComponent conditionManager
    { get => _conditionManager ?? _player.GetCoreComponent(ref _conditionManager); }
    private TemporaryComponent _conditionManager;

    protected WeaponHandler weaponHandler
    { get => _weaponHandler ?? _player.GetCoreComponent(ref _weaponHandler); }
    private WeaponHandler _weaponHandler;

    [SerializeField] protected InputTransitionCondition _isPressingAttackButtonSO;
    [SerializeField] protected ScriptableVector3 _mousePositionSO;

    protected Animator _weaponAnimator;
    protected Core _player;
    protected Vector2 _aimDirection;

    protected bool _isPressingAttackButton => _isPressingAttackButtonSO.value;
    protected Vector3 _mousePosition => _mousePositionSO.value;

    protected float _aimAngle;

    protected virtual void Awake()
    {
        _weaponAnimator = GetComponent<Animator>();
        _player = GetComponentInParent<Core>();
    }

    public virtual void LogicUpdate()
    {
        HandleAiming();
        HandlePlayerFacingDirection();
    }

    public void Flip(float flipAngle)
    {
        _weaponAnimator.transform.localRotation = Quaternion.Euler(flipAngle, flipAngle, 0f);
    }

    private void HandleAiming()
    {
        _aimDirection = (_mousePosition - weaponHandler.transform.position).normalized;
        _aimAngle = Vector2.SignedAngle(Vector2.right, _aimDirection);
        weaponHandler.transform.localRotation = Quaternion.Euler(0f, 0f, _aimAngle);
    }

    private void HandlePlayerFacingDirection()
    {
        conditionManager.CheckFacingDirection(_mousePosition, _player.transform.position);
    }
}
