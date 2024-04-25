using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class SettlerMoveState : SettlerBaseState
{
	private Vector2 _movePos;
	
	public SettlerMoveState(SettlerStateMachine currentContext, SettlerStateFactory settlerStateFactory) : base(currentContext, settlerStateFactory) { }
	
	public override void CheckSwitchStates()
	{
		if (!Ctx.AI.pathPending && (Ctx.AI.reachedEndOfPath || !Ctx.AI.hasPath)) {
			SwitchState(Factory.Idle());
		}
	}

	public override void EnterState()
	{
		Ctx.AI.isStopped = false;
		_movePos = CalcWanderPos();
		Ctx.AI.destination = _movePos;
		Ctx.AI.SearchPath();
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
	
	private Vector2 CalcWanderPos()
	{
		if(Vector2.Distance(Ctx.transform.position, Ctx.PlacePoint) > 5)
			return Ctx.PlacePoint;
		
		GraphNode startNode = AstarPath.active.GetNearest(Ctx.transform.position, NNConstraint.Default).node; 

		List<GraphNode> nodes = PathUtilities.BFS(startNode, 3); 
		Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0]; 
		
		return singleRandomPoint;
	}
}
