using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlerIdleState : SettlerBaseState
{
	public SettlerIdleState(SettlerStateMachine currentContext, SettlerStateFactory settlerStateFactory) : base(currentContext, settlerStateFactory) { }
	
	public override void CheckSwitchStates()
	{
		if(!Ctx.IsIdle)
			SwitchState(Factory.Move());
	}

	public override void EnterState()
	{
		Ctx.IsIdle = true;
		Ctx.StartCoroutine(Ctx.IdleDuration());
	}

	public override void ExitState()
	{
		
	}

	public override void InitializeSubState()
	{
		
	}

	public override void UpdateState()
	{
		CheckSwitchStates();
	}
}
