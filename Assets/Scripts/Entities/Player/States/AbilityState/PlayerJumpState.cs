using UnityEngine;

[CreateAssetMenu(fileName = "Player Jump State", menuName = "States/Player/Ability/Jump State")]


public class PlayerJumpState : PlayerAbilityState
{
    public override void Enter()
    {
        base.Enter();

        inputHandler.UseJumpInput();
        conditionManager.DecreaseAmountOfJumpsLeft();
        
        movement.SetVelocityY(_playerData.jumpVelocity);

        conditionManager.IsJumpingSO.value = true;

        _isAbilityDone = true;
    }

}