using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class SettlerStateMachine : MonoBehaviour
{
	private SettlerBaseState _currentState;
	private SettlerStateFactory _states;
	private Vector2 _placePoint;
	private IAstarAI _ai;
	private Rigidbody2D _rb;
	private bool _isIdle;
	
	public SettlerBaseState CurrentState { get { return _currentState; } set { _currentState = value;} }
	public Vector2 PlacePoint => _placePoint;
	public IAstarAI AI => _ai;
	public Rigidbody2D Rb => _rb;
	public bool IsIdle { get { return _isIdle; } set { _isIdle = value;} }
	
	private void Awake()
	{
		_states = new SettlerStateFactory(this);
		_ai = GetComponent<IAstarAI>();
		_rb = GetComponent<Rigidbody2D>();
	}
	
	private void Start()
	{
		_currentState = _states.Grounded();
		_currentState.EnterState();
		
		_placePoint = transform.position;
	}
	
	private void LateUpdate()
	{
		_currentState.UpdateStates();
	}
	
	public IEnumerator IdleDuration()
	{
		_isIdle = true;
		yield return new WaitForSeconds(Random.Range(8,16));
		_isIdle = false;
	}
}
