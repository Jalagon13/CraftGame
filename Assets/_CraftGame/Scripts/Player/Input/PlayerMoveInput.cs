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
	private TimeController _timeController;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.Move.performed += MovementAction;
		_playerInput.Player.Move.canceled += MovementAction;
		_playerInput.Enable();
		
		_rb = GetComponent<Rigidbody2D>();
		_timeController = GetComponent<TimeController>();
		
		GameSignals.ON_UI_ACTIVATED.AddListener(DisableControl);
		GameSignals.ON_UI_UNACTIVED.AddListener(EnableControl);
		GameSignals.ON_DAY_END.AddListener(DisableControl);
		GameSignals.ON_DAY_START.AddListener(EnableControl);
	}
	
	private void OnDestroy()
	{
		_playerInput.Disable();
		
		GameSignals.ON_UI_ACTIVATED.RemoveListener(DisableControl);
		GameSignals.ON_UI_UNACTIVED.RemoveListener(EnableControl);
		GameSignals.ON_DAY_END.RemoveListener(DisableControl);
		GameSignals.ON_DAY_START.RemoveListener(EnableControl);
	}
	
	private void Start()
	{
		_mainCamera = Camera.main;
	}
	
	private void FixedUpdate()
	{
		_rb.MovePosition(_rb.position + _moveDirection * (_timeController.NoMoreEnergy() ? 2.5f : _speed) * Time.deltaTime);
		_po.Position = transform.position;
		_po.MousePosition = (Vector2)_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
	}
	
	private void DisableControl(ISignalParameters parameters)
	{
		_playerInput.Disable();
	}
	
	private void EnableControl(ISignalParameters parameters)
	{
		_playerInput.Enable();
	}
	
	private void MovementAction(InputAction.CallbackContext context)
	{
		// if(!_canMove) return;
		
		_moveDirection = context.ReadValue<Vector2>();
	}
}
