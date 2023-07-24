using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class VisualController : CoreComponent
{
    private Transform _facingDirectionIndicator;
    private Animator _animator;
    private SpriteRenderer _sprite;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _animator = GetComponent<Animator>();
        _facingDirectionIndicator = transform.Find("FacingDirectionIndicator");
    }

    public void FlipEntity(int facingDirection)
    {
        _facingDirectionIndicator.Rotate(0f, 180 * facingDirection, 0f);
        //_sprite.flipX = false;
    }

    public void SetAnimationBool(string animBoolName, bool value)
    {
        _animator.SetBool(animBoolName, value);
    }

    public void SetAnimationFloat(string animFloatName, float value)
    {
        _animator.SetFloat(animFloatName, value);
    }

    public void AnimationTrigger() { }
    public void AnimationFinishedTrigger() { }
}
