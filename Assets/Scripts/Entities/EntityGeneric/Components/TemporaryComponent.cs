using UnityEngine;
using System;

public class TemporaryComponent : CoreComponent
{
    [SerializeField] private PlayerInputHandler _brain;
    [SerializeField] private PlayerConditionTable _conditions;
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private Transform _ceilingCheckTransform;

    private WeaponHandler _weaponHandler;
    private VisualController _visualController;
    private Collisions _collisions;
    private Movement _movement;
    private SoundComponent _sound;

    private Camera _mainCamera;
    private float _stepDelay;

    private Vector2 _rawMouseInput;
    private PlayerCrouchingForm _crouchingForm;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);
        
        _mainCamera = Camera.main;
        _crouchingForm = PlayerCrouchingForm.notCrouching;
        _conditions.FacingDir = 1;
        _conditions.MovementDir = 1;
        _conditions.JumpInputStartTime = Time.time;
        _conditions.NumberOfJumpsLeft = _playerData.numberOfJumps;
        _conditions.NumberOfCrouchesLeft = _playerData.numberOfCrouches;
    }

    public override void SetupConnections()
    {
        base.SetupConnections();

        _weaponHandler = _core.GetCoreComponent<WeaponHandler>();
        _visualController = _core.GetCoreComponent<VisualController>();
        _collisions = _core.GetCoreComponent<Collisions>();
        _movement = _core.GetCoreComponent<Movement>();
        _sound = _core.GetCoreComponent<SoundComponent>();
    }

    private void OnEnable()
    {
        _brain.AimEvent += OnAim;
        _brain.JumpEvent += OnJump;
        _brain.GrabEvent += OnGrab;
        _brain.CrouchEvent += OnCrouch;
        _brain.MoveEvent += ProcessMovement;
        _brain.WeaponSwitchEvent += OnWeaponSwitch;
        _brain.JumpCanceledEvent += OnJumpCancelled;
        _brain.PrimaryAttackEvent += OnPrimaryAttack;
        _brain.SecondaryAttackEvent += OnSecondaryAttack;
    }

    private void OnDisable()
    {
        _brain.AimEvent -= OnAim;
        _brain.JumpEvent -= OnJump;
        _brain.GrabEvent -= OnGrab;
        _brain.CrouchEvent -= OnCrouch;
        _brain.MoveEvent -= ProcessMovement;
        _brain.WeaponSwitchEvent -= OnWeaponSwitch;
        _brain.JumpCanceledEvent -= OnJumpCancelled;
        _brain.PrimaryAttackEvent -= OnPrimaryAttack;
        _brain.SecondaryAttackEvent -= OnSecondaryAttack;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateMouseInput();
    }

#region Actions

    public void ClimbLedge()
    {
        _movement.SetVelocityY(_playerData.ledgeClimbVelocity);
    }

    public void Crouch()
    {
        if (_crouchingForm == PlayerCrouchingForm.notCrouching && _conditions.IsPressingCrouch)
        {
            float biggerHeight = _playerData.standColliderHeight;
            float smallerHeight = _playerData.crouchColliderHeight;

            _crouchingForm = PlayerCrouchingForm.crouchingDown;

            float height = _collisions.ColliderSize.y * smallerHeight;

            _collisions.SetColliderOffsetY((height - _collisions.ColliderSize.y) / 2);
            _collisions.SetColliderSizeY(height);

            float moveDistance = (_playerData.crouchHeightDifference * _collisions.DefaultSize.y);
            _ceilingCheckTransform.position -= Vector3.up * moveDistance;
        }
    }

    public void CrouchInAir()
    {
        _conditions.NumberOfCrouchesLeft--;
        Crouch();
    }

    public void UnCrouch()
    {
        if (((_crouchingForm == PlayerCrouchingForm.crouchingDown && !_conditions.IsPressingCrouch)
        || !_conditions.IsTouchingWall) && !_conditions.IsTouchingCeiling)
        {
            float biggerHeight = _playerData.standColliderHeight;
            float smallerHeight = _playerData.crouchColliderHeight;

            _crouchingForm = PlayerCrouchingForm.notCrouching;

            _collisions.SetColliderOffset(Vector2.zero);
            _collisions.SetColliderSize(_collisions.DefaultSize);

            float moveDistance = (_playerData.crouchHeightDifference * _collisions.DefaultSize.y);
            _ceilingCheckTransform.position += Vector3.up * moveDistance;
        }
    }

    public void Jump()
    {
        if (_sound.jumpSound)
            _sound.jumpSound.Play();

        _movement.SetVelocityY(_playerData.jumpVelocity);

        _conditions.NumberOfJumpsLeft--;
        _conditions.IsJumping = true;
        _conditions.IsPressingJump = false;
    }

    public void WallJump()
    {
        if (_sound.jumpSound)
            _sound.jumpSound.Play();

        _conditions.MovementDir = (_conditions.IsTouchingWall ? -1 : 1) * _conditions.MovementDir;
        UpdateMovementDirection();
        
        Vector2 wallJumpDirection = (Vector2)(Quaternion.Euler(0, 0, _playerData.wallJumpAngle) * Vector2.right); //Temporary
        _movement.SetVelocityAtAngle(_playerData.wallJumpVelocity, wallJumpDirection, _conditions.MovementDir);

        _conditions.NumberOfJumpsLeft = _playerData.numberOfJumps;
        _conditions.NumberOfJumpsLeft--;
        _conditions.IsPressingJump = false;
    }

    public void StopMoving()
    {
        _movement.SetVelocityZero();
    }

    public void Land()
    {
        if (_sound.fallSound)
            _sound.fallSound.Play();

        _conditions.NumberOfJumpsLeft = _playerData.numberOfJumps;
        _conditions.NumberOfCrouchesLeft = _playerData.numberOfCrouches;
    }

    public void Step()
    {
        _conditions.LastStepTime = Time.time;

        if (_sound.moveSound)
            _sound.moveSound.Play();
    }

    public void StepContinuous()
    {
        if (_conditions.LastStepTime + _stepDelay >= Time.time)
            Step();
    }

    public void SlideDown()
    {
        Step();
        _movement.SetVelocityY(-_playerData.wallSlideVelocity);
    }
    
    public void ClimbWall()
    {
        StepContinuous();
        _movement.SetVelocityY(_playerData.wallClimbVelocity);
    }

    public void MoveOnGroundCrouched()
    {
        StepContinuous();
        _movement.SetVelocityX(_playerData.crouchMovementVelocity * _conditions.MovementDir);
    }

    public void MoveOnGround()
    {
        StepContinuous();
        _movement.SetVelocityX(_playerData.movementVelocity * _conditions.NormalizedInputX);
        //movement.AddForceX(_playerData.movementVelocity * _inputX, ForceMode2D.Impulse);
    }

    public void MoveInAir()
    {
        if (_conditions.NormalizedInputX != 0)
            _movement.SetVelocityX(_playerData.movementVelocity * _conditions.NormalizedInputX * _playerData.defaultAirControlPercentage);

        _visualController?.SetAnimationFloat("velocityX", Mathf.Abs(_movement.CurrentVelocity.x));
        _visualController?.SetAnimationFloat("velocityY", _movement.CurrentVelocity.y);
    }

    public void StopMovementSound()
    {
        _sound.moveSound.Stop();
    }

    public void HoldPosition()
    {
        //_core.transform.position = _conditions.HoldPosition;

        //StopMoving();
    }
    
    public void FreezeInPlace()
    {
        _movement.SetGravity(0f);

        StopMoving();
    }
    
    public void LetGoOfWall()
    {
        _movement.SetGravity(2f);
    }

    public void Flip()
    {
        _conditions.FacingDir *= -1;

        _weaponHandler?.FlipWeapon(_conditions.FacingDir);
        _visualController?.FlipEntity(_conditions.FacingDir);
    }

    public void StartCoyoteTime()
    {
        _conditions.IsInCoyoteTime = true;
        _conditions.CoyoteTimeStartTime = Time.time;
    }

    public void UpdateMovementDirection()
    {
        if (_conditions.IsMovingX && !_conditions.IsMovingInCorrectDir)
            _conditions.MovementDir *= -1;
    }

    public void UpdateFacingDirection()
    {
        Vector2 mouseDirection = (_conditions.MousePosition - _core.transform.position).normalized;

        float angle = Vector2.SignedAngle(Vector2.right, mouseDirection);
        angle = (angle > 90) ? angle - 270 : angle + 90;

        if (Math.Sign(angle) != _conditions.FacingDir)
            Flip();
    }

    public void UpdateCoyoteTime()
    {
        if (_conditions.IsInCoyoteTime && Time.time > _conditions.CoyoteTimeStartTime + _playerData.coyoteTime)
        {
            _conditions.IsInCoyoteTime = false;
            _conditions.NumberOfJumpsLeft--;
        }
    }

    public void UpdateJumpStatus()
    {
        if (!_conditions.IsJumping)
            return;

        if (_conditions.IsJumpCanceled)
        {
            _movement.SetVelocityY(_movement.CurrentVelocity.y * _playerData.variableJumpHeightMultiplier);
            _conditions.IsJumping = false;
        }
        else if (_movement.CurrentVelocity.y <= 0f)
        {
            _conditions.IsJumping = false;
        }

        if (Time.time >= (_conditions.JumpInputStartTime + _playerData.jumpInputHoldTime))
            _conditions.IsPressingJump = false;
    }

    public void UpdateFallStatus()
    {
        _conditions.HasStoppedFalling = _movement.CurrentVelocity.y < 0.01;
    }

#endregion

#region Brain Input

    public void ProcessMovement(Vector2 value)
    {
        _conditions.NormalizedInputX = Mathf.RoundToInt(value.x);
        _conditions.NormalizedInputY = Mathf.RoundToInt(value.y);
    }

    public void OnJump()
    {
        _conditions.IsPressingJump = true;
        
        _conditions.IsJumpCanceled = false;

        _conditions.JumpInputStartTime = Time.time;
    }

    public void OnJumpCancelled()
    {
        _conditions.IsPressingJump = false;
        _conditions.IsJumpCanceled = true;
    }

    public void OnGrab(bool value)
    {
        _conditions.IsPressingGrab = value;
    }

    public void OnPrimaryAttack(bool value)
    {
        _conditions.IsPressingPrimaryAttack = value;
    }

    public void OnSecondaryAttack(bool value)
    {
        _conditions.IsPressingSecondaryAttack = value;
    }

    public void OnCrouch(bool value)
    {
        _conditions.IsPressingCrouch = value;
    }

    public void OnAim(Vector2 value)
    {
        _rawMouseInput = value;
    }

    public void OnWeaponSwitch(Vector2 value)
    {
        _conditions.WeaponSwitchInput = (int)value.y;
    }

    public void OnPause()
    {

    }

#endregion
    
    private void UpdateMouseInput()
    {
        Vector3 shiftedMouseInput = new Vector3(_rawMouseInput.x, _rawMouseInput.y, 10);

        _conditions.MousePosition = _mainCamera.ScreenToWorldPoint(shiftedMouseInput);
    }

    public enum PlayerCrouchingForm
    {
        notCrouching,
        crouchingDown,
        crouchingUp
    }
}