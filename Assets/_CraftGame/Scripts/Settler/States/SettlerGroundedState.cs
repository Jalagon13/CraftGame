using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlerGroundedState : SettlerBaseState
{
    public SettlerGroundedState(SettlerStateMachine currentContext, SettlerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Idle());
    }

    public override void CheckSwitchStates()
    {
        
    }
}
