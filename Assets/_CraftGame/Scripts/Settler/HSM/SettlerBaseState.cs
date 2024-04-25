using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SettlerBaseState
{
    private SettlerStateMachine _ctx;
    private SettlerStateFactory _factory;
    private SettlerBaseState _currentSubState;
    private SettlerBaseState _currentSuperState;

    private bool _isRootState = false;

    protected bool IsRootState { set { _isRootState = value; } }
    protected SettlerStateMachine Ctx { get { return _ctx; } }
    protected SettlerStateFactory Factory { get { return _factory; } }

    public SettlerBaseState(SettlerStateMachine currentContext, SettlerStateFactory settlerStateFactory)
    {
        _ctx = currentContext;
        _factory = settlerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateStates();
    }
    protected void SwitchState(SettlerBaseState newState)
    {
        // current state exits state
        ExitState();

        // new state enters state
        newState.EnterState();

        if (_isRootState)
        {
            // switch current state of context
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            // set the current super state sub state to the new state
            _currentSuperState.SetSubState(newState);
        }
    }
    protected void SetSuperState(SettlerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(SettlerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
