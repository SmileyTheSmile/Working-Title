using UnityEngine;
using System;

public class Player : CoreComponent
{
    [SerializeField] private PlayerInputHandler _brain;
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private Transform _ceilingCheckTransform;

    private WeaponHandler _weaponHandler;
    private VisualController _visualController;
    private Collisions _collisions;
    private Movement _movement;
    private SoundComponent _sound;

    public override void Initialize(Core core)
    {
        base.Initialize(core); 

        _stats.NumberOfJumpsLeft = _playerData.numberOfJumps;
        _stats.NumberOfInAirCrouchesLeft = _playerData.numberOfCrouches;

        SetupConnections();
        SetupStates();
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

    private void SetupStates()
    {

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


#region Actions

    //Movement, PlayerData
    public void ClimbLedge()
    {
        _movement.SetVelocityY(_playerData.ledgeClimbVelocity);
    }

    //Collisions, PlayerData, CollisionSenses
    public void Crouch()
    {
        if (!_stats.IsCrouchingDown)
        {
            _stats.IsCrouchingDown = true;

            float height = _collisions.ColliderSize.y * _playerData.crouchColliderHeight;
            _collisions.SetColliderOffsetY((height - _collisions.ColliderSize.y) / 2);
            _collisions.SetColliderHeight(height);

            float moveDistance = _collisions.DefaultSize.y * _playerData.crouchHeightDifference;
            _ceilingCheckTransform.position -= Vector3.up * moveDistance;
        }
    }

    //Collisions, PlayerData, Conditions, CollisionSenses
    public void CrouchInAir()
    {
        _stats.NumberOfInAirCrouchesLeft--;
        Crouch();
    }

    //Collisions, PlayerData, CollisionSenses
    public void UnCrouch()
    {
        if (_stats.IsCrouchingDown)
        {
            _stats.IsCrouchingDown = false;

            _collisions.SetColliderOffset(Vector2.zero);
            _collisions.SetColliderSize(_collisions.DefaultSize);

            float moveDistance = (_playerData.crouchHeightDifference * _collisions.DefaultSize.y);
            _ceilingCheckTransform.position += Vector3.up * moveDistance;
        }
    }

    //Sound, Conditions, Movement
    public void Jump()
    {
        if (_sound.jumpSound)
            _sound.jumpSound.Play();

        _stats.NumberOfJumpsLeft--;
        _stats.IsJumping = true;
        _stats.IsPressingJump = false;

        _movement.SetVelocityY(_playerData.jumpVelocity);
    }

    //Sound, Conditions, Movement
    public void WallJump()
    {
        if (_sound.jumpSound)
            _sound.jumpSound.Play();

        _stats.MovementDir = (_stats.IsTouchingWall ? -1 : 1) * _stats.MovementDir;
        _stats.NumberOfJumpsLeft = _playerData.numberOfJumps;
        _stats.NumberOfJumpsLeft--;
        _stats.IsPressingJump = false;
        UpdateMovementDirection();
        
        Vector2 wallJumpDirection = (Vector2)(Quaternion.Euler(0, 0, _playerData.wallJumpAngle) * Vector2.right); //Temporary
        _movement.SetVelocityAtAngle(_playerData.wallJumpVelocity, wallJumpDirection, _stats.MovementDir);

    }

    //Movement
    public void StopMoving()
    {
        _movement.SetVelocityZero();
    }

    //Sound, Conditions
    public void Land()
    {
        if (_sound.fallSound)
            _sound.fallSound.Play();

        _stats.NumberOfJumpsLeft = _playerData.numberOfJumps;
        _stats.NumberOfInAirCrouchesLeft = _playerData.numberOfCrouches;
    }

    //Sound, Conditions
    public void Step()
    {
        if (_sound.moveSound)
            _sound.moveSound.Play();

        _stats.LastStepTime = Time.time;
    }

    //Sound, Conditions
    public void StepContinuous()
    {
        if (_stats.LastStepTime + _playerData.StepDelay <= Time.time)
            Step();
    }

    //Sound, Conditions, Movement
    public void SlideDown()
    {
        StepContinuous();
        _movement.SetVelocityY(-_playerData.wallSlideVelocity);
    }
    
    //Sound, Conditions, Movement
    public void ClimbWall()
    {
        StepContinuous();
        _movement.SetVelocityY(_playerData.wallClimbVelocity);
    }

    //Sound, Conditions, Movement
    public void MoveOnGroundCrouched()
    {
        StepContinuous();
        _movement.SetVelocityX(_playerData.crouchMovementVelocity * _stats.MovementDir);
    }

    //Sound, Stats, Movement
    public void MoveOnGround()
    {
        StepContinuous();
        _movement.SetVelocityX(_playerData.movementVelocity * _stats.NormalizedInputX);
        //movement.AddForceX(_playerData.movementVelocity * _inputX, ForceMode2D.Impulse);
    }

    //Movement, Visuals
    public void MoveInAir()
    {
        if (_stats.IsMovingX)
            _movement.SetVelocityX(_playerData.movementVelocity * _stats.NormalizedInputX * _playerData.defaultAirControlPercentage);

        _visualController?.SetAnimationFloat("velocityX", Mathf.Abs(_movement.CurrentVelocity.x));
        _visualController?.SetAnimationFloat("velocityY", _movement.CurrentVelocity.y);
    }

    //Sound
    public void StopMovementSound()
    {
        _sound.moveSound.Stop();
    }

    //Movement
    public void FreezeInPlace()
    {
        _movement.SetGravity(0f);

        StopMoving();
    }
    
    //Movement
    public void LetGoOfWall()
    {
        _movement.SetGravity(2f);
    }

    //Stats, WeaponHandler, Visuals
    public void Flip()
    {
        _stats.FacingDir *= -1;

        _weaponHandler?.FlipWeapon(_stats.FacingDir);
        _visualController?.FlipEntity(_stats.FacingDir);
    }

    //Stats
    public void StartCoyoteTime()
    {
        _stats.IsInCoyoteTime = true;
        _stats.CoyoteTimeStartTime = Time.time;
    }

    //Stats
    public void UpdateMovementDirection()
    {
        if (_stats.IsMovingX && !_stats.IsMovingInCorrectDir)
            _stats.MovementDir *= -1;
    }

    //Core, Stats
    public void UpdateFacingDirection()
    {
        Vector2 mouseDirection = (_stats.MousePosition - _core.transform.position).normalized;

        float angle = Vector2.SignedAngle(Vector2.right, mouseDirection);
        angle = (angle > 90) ? angle - 270 : angle + 90;

        if (Math.Sign(angle) != _stats.FacingDir)
            Flip();
    }

    //Stats, PlayerData
    public void UpdateCoyoteTime()
    {
        if (_stats.IsInCoyoteTime && Time.time > _stats.CoyoteTimeStartTime + _playerData.coyoteTime)
        {
            _stats.IsInCoyoteTime = false;
            _stats.NumberOfJumpsLeft--;
        }
    }

    //Stats, Movement, PlayerData
    public void UpdateJumpStatus()
    {
        if (!_stats.IsJumping)
            return;

        if (_stats.IsJumpCanceled)
        {
            _movement.SetVelocityY(_movement.CurrentVelocity.y * _playerData.variableJumpHeightMultiplier);
            _stats.IsJumping = false;
        }
        else if (_movement.CurrentVelocity.y <= 0f)
        {
            _stats.IsJumping = false;
        }
        else if (Time.time >= (_stats.JumpInputStartTime + _playerData.jumpInputHoldTime))
        {
            _stats.IsPressingJump = false;
        }
    }

    //Stats, Movement
    public void UpdateFallStatus()
    {
        _stats.HasStoppedFalling = _movement.CurrentVelocity.y < 0.01;
    }

#endregion


#region Brain Input

    public void ProcessMovement(Vector2 value)
    {
        _stats.NormalizedInputX = Mathf.RoundToInt(value.x);
        _stats.NormalizedInputY = Mathf.RoundToInt(value.y);
    }

    public void OnJump()
    {
        _stats.IsPressingJump = true;
        _stats.IsJumpCanceled = false;
        _stats.JumpInputStartTime = Time.time;
    }

    public void OnJumpCancelled()
    {
        _stats.IsPressingJump = false;
        _stats.IsJumpCanceled = true;
    }

    public void OnGrab(bool value)
    {
        _stats.IsPressingGrab = value;
    }

    public void OnPrimaryAttack(bool value)
    {
        _stats.IsPressingPrimaryAttack = value;
    }

    public void OnSecondaryAttack(bool value)
    {
        _stats.IsPressingSecondaryAttack = value;
    }

    public void OnCrouch(bool value)
    {
        _stats.IsPressingCrouch = value;
    }

    public void OnAim(Vector2 value)
    {
        _stats.RawMousePosition = value;
    }

    public void OnWeaponSwitch(Vector2 value)
    {
        _stats.WeaponSwitchInput = (int)value.y;
    }

    public void OnPause()
    {

    }

#endregion


}