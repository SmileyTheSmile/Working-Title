using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class VisualController : CoreComponent
{
    private Transform _facingDirectionIndicator;
    private Animator _animator;
    private SpriteRenderer _sprite;

    public override void Initialize(EntityCore entity)
    {
        base.Initialize(entity);

        _animator = GetComponent<Animator>();
        _facingDirectionIndicator = transform.Find("FacingDirectionIndicator");
    }

    //Flip the entity left or right
    public void FlipEntity(int facingDirection)
    {
        _facingDirectionIndicator.Rotate(0f, 180 * facingDirection, 0f);
        //_sprite.flipX = false;
    }

    //Set the animation bool in the animator
    public void SetAnimationBool(string animBoolName, bool value)
    {
        _animator.SetBool(animBoolName, value);
    }

    //Set the animation float in the animator
    public void SetAnimationFloat(string animFloatName, float value)
    {
        _animator.SetFloat(animFloatName, value);
    }

    public void AnimationTrigger() { }
    public void AnimationFinishedTrigger() { }
}
