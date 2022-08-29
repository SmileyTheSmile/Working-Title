using System.ComponentModel.Design.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Jump State", menuName = "States/Player/Ability/Jump State")]

public class PlayerJumpState : PlayerAbilityState
{
    [SerializeField] protected SupportTransitionCondition IsJumpingSO;

    public override void Enter()
    {
        base.Enter();

        _temporaryComponent.UseJumpInput();
        _temporaryComponent.DecreaseAmountOfJumpsLeft();
        
        _movement.SetVelocityY(_playerData.jumpVelocity);

        IsJumpingSO.value = true;
    }
}