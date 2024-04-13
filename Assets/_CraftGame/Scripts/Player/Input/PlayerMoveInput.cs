using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveInput : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private float _speed;
	
	private Rigidbody2D _rb;
	private PlayerInput _playerInput;
	private Vector2 _moveDirection;
	private Camera _mainCamera;
	private EnergyController _energyController;
	
	private void Awake()
	{
		_playerInput = new();
		_rb = GetComponent<Rigidbody2D>();
		_energyController = GetComponent<EnergyController>();
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
		_mainCamera = Camera.main;
	}
	
	private void FixedUpdate()
	{
		if(_energyController.CurrentEnergy <= 0)
		{
			_rb.MovePosition(_rb.position + _moveDirection * 2.5f * Time.deltaTime);
		}
		else
		{
			_rb.MovePosition(_rb.position + _moveDirection * _speed * Time.deltaTime);
		}
		
		
		
		_po.Position = transform.position;
		_po.MousePosition = (Vector2)_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
	}
	
	private void MovementAction(InputAction.CallbackContext context)
		{
			// if(!_canMove) return;
			
			_moveDirection = context.ReadValue<Vector2>();
		}
}
