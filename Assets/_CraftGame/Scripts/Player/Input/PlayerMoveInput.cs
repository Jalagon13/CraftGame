using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveInput : MonoBehaviour
{
	[SerializeField] private float _speed;
	
	private Rigidbody2D _rb;
	private PlayerInput _playerInput;
	private Vector2 _moveDirection;
	
	private void Awake()
	{
		_playerInput = new();
		_rb = GetComponent<Rigidbody2D>();
	}
	
	private void OnEnable()
	{
		_playerInput.Enable();
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
	}
	
	private void Start()
	{
		_playerInput.Player.Move.performed += MovementAction;
		_playerInput.Player.Move.canceled += MovementAction;
	}
	
	private void FixedUpdate()
	{
		_rb.MovePosition(_rb.position + _moveDirection * _speed * Time.deltaTime);
	}
	
	private void MovementAction(InputAction.CallbackContext context)
		{
			// if(!_canMove) return;
			
			_moveDirection = context.ReadValue<Vector2>();
		}
}
