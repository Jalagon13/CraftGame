using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControl : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	
	private PlayerInput _playerInput;
	private Clickable _currentClickable;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.PrimaryAction.started += Hit;
	}
	
	private void OnEnable()
	{
		_playerInput.Enable();
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
	}
	
	private void LateUpdate()
	{
		transform.SetPositionAndRotation(CalcPosition(), Quaternion.identity);
		
		UpdateCurrentClickable();
	}
	
	private void Hit(InputAction.CallbackContext context)
	{
		if(_currentClickable != null)
		{
			_currentClickable.Hit(1);
			GameSignals.CLICKABLE_CLICKED.Dispatch();
		}
	}
	
	private void UpdateCurrentClickable()
	{
		Clickable lastestClickable = ClickableFound();

		if (lastestClickable != null)
		{
			if (_currentClickable == lastestClickable) return;

			_currentClickable = lastestClickable;
		}
		else
		{
			if (_currentClickable == null) return;
			_currentClickable = null;
		}
	}

	private Clickable ClickableFound()
	{
		Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
		List<Clickable> clickablesFound = new();

		if (colliders.Count() > 0)
		{
			foreach (Collider2D c in colliders)
			{
				if (c.TryGetComponent(out Clickable clickable))
				{
					clickablesFound.Add(clickable);
				}
			}
		}

		return clickablesFound.Count > 0 ? clickablesFound.Last() : null;
	}
	
	private Vector2 CalcPosition()
	{
		Vector2 taPosition;
		Vector2 playerPos = transform.root.transform.localPosition + new Vector3(0, -0.3f, 0);
		Vector2 direction = (_po.MousePosition - playerPos).normalized;

		taPosition = Vector2.Distance(playerPos, _po.MousePosition) > 1 ? (playerPos += new Vector2(0, 0.25f)) + (direction * 1) : _po.MousePosition;

		return taPosition;
	}
}
