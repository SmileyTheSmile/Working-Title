using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    private string animBoolNime;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolNime) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolNime = animBoolNime;
    }

    public virtual void Enter() {
        DoChecks();
        startTime = Time.time;
    }

    public virtual void Exit() {
        
    }

    public virtual void LogicUpdate() {
        
    }

    public virtual void PhysicsUpdate() {
        DoChecks();
    }

    public virtual void DoChecks() {
        
    }
}
