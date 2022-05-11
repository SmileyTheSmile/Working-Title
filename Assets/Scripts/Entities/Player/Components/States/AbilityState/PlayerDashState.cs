using UnityEngine;

[CreateAssetMenu(fileName = "Player Dash State", menuName = "States/Player/Ability/Dash State")]

public class PlayerDashState : PlayerAbilityState
{
    private Vector2 _dashDirectionInput;
    private Vector2 _dashDirection;

    private float _lastDashTime;

    private bool _dashInputStop;
    private bool _canDash;
    private bool _isHolding;

    public PlayerDashState(Player player, PlayerData playerData, string animBoolName)
    : base(player, animBoolName, playerData) { }

    public override void Enter()
    {
        base.Enter();

        _isHolding = true;
        _canDash = false;

        inputHandler?.UseDashInput();
        _dashDirection = Vector2.right * movement._movementDir;

        Time.timeScale = _playerData.holdTimeScale;
        _startTime = Time.unscaledTime;
    }

    public override void Exit()
    {
        base.Exit();


        if (movement._currentVelocity.y > 0)
        {
            movement?.SetVelocityY(movement._currentVelocity.y * _playerData.dashEndYMultiplier);
        }
    }

    public override void DoActions()
    {
        base.DoActions();

        visualController?.SetAnimationFloat("velocityY", movement._currentVelocity.y);
        visualController?.SetAnimationFloat("velocityX", Mathf.Abs(movement._currentVelocity.x));

        if (!_isHolding)
        {
            movement?.SetVelocity(_playerData.dashVelocity, _dashDirection);

            if (Time.time >= _startTime + _playerData.dashTime)
            {
                movement?.SetDrag(0f);
                _isAbilityDone = true;
                _lastDashTime = Time.time;
            }

            return;
        }

        _dashDirectionInput = inputHandler.mousePositionInput - _player.transform.position;
        _dashInputStop = inputHandler.dashInputStop;

        if (_dashDirectionInput != Vector2.zero)
        {
            _dashDirection = _dashDirectionInput;
            _dashDirection.Normalize();
        }

        float angle = Vector2.SignedAngle(Vector2.right, _dashDirection);

        if (_dashInputStop || Time.unscaledTime >= _startTime + _playerData.maxHoldTime)
        {
            _isHolding = false;
            Time.timeScale = 1f;
            _startTime = Time.time;

            movement?.CheckMovementDirection(Mathf.RoundToInt(_dashDirection.x));
            movement?.SetVelocity(_playerData.dashVelocity, _dashDirection);

            movement?.SetDrag(_playerData.drag);
        }
    }

    public bool CheckIfCanDash()
    {
        return _canDash && Time.time >= _lastDashTime + _playerData.dashCooldown;
    }

    public void ResetCanDash() => _canDash = true;
}
