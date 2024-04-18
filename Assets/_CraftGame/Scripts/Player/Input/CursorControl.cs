using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControl : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private ItemParameter _damageMinParameter;
	[SerializeField] private ItemParameter _damageMaxParameter;
	
	private PlayerInput _playerInput;
	private Clickable _currentClickable;
	private InventoryItem _focusInventoryItem;
	private int _damageMin;
	private int _damageMax;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.PrimaryAction.started += Hit;
		_playerInput.Player.Interact.started += TryToInteract;
		_playerInput.Enable();
		
		GameSignals.FOCUS_INVENTORY_ITEM_UPDATED.AddListener(FocusInventoryItemUpdated);
		GameSignals.ON_UI_ACTIVATED.AddListener(DisableControl);
		GameSignals.ON_UI_UNACTIVED.AddListener(EnableControl);
	}
	
	private void OnDestroy()
	{
		_playerInput.Disable();
		
		GameSignals.FOCUS_INVENTORY_ITEM_UPDATED.RemoveListener(FocusInventoryItemUpdated);
		GameSignals.ON_UI_ACTIVATED.RemoveListener(DisableControl);
		GameSignals.ON_UI_UNACTIVED.RemoveListener(EnableControl);
	}
	
	private void LateUpdate()
	{
		transform.SetPositionAndRotation(CalcPosition(), Quaternion.identity);
		
		UpdateCurrentClickable();
	}
	
	private void DisableControl(ISignalParameters parameters)
	{
		_playerInput.Disable();
	}
	
	private void EnableControl(ISignalParameters parameters)
	{
		_playerInput.Enable();
	}
	
	private void TryToInteract(InputAction.CallbackContext context)
	{
		if(_po.SomeUiActive) return;
		
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0);

		foreach(Collider2D col in colliders)
		{
			if(col.TryGetComponent(out IInteractable interactable)) 
			{
				interactable.OnInteract();
				return;
			}
		}
	}
	
	private void FocusInventoryItemUpdated(ISignalParameters parameters) 
	{
		_focusInventoryItem = (InventoryItem)parameters.GetParameter("FocusInventoryItem");
	}
	
	private void Hit(InputAction.CallbackContext context)
	{
		if(_currentClickable == null || Pointer.IsOverUI() || _focusInventoryItem.Item is not ToolObject) return;
		
		ToolObject tool = _focusInventoryItem.Item as ToolObject;
		
		_damageMin = ExtractParameterValue(_damageMinParameter);
		_damageMax = ExtractParameterValue(_damageMaxParameter);
		
		System.Random rnd = new System.Random();
		var damage = rnd.Next(_damageMin, _damageMax + 1); 
		
		_currentClickable.Hit(damage, tool.ToolType);
	}
	
	private int ExtractParameterValue(ItemParameter paramter)
	{
		var itemParams = _focusInventoryItem.Item.DefaultParameterList;

		if (itemParams.Contains(paramter))
		{
			int index = itemParams.IndexOf(paramter);
			return (int)itemParams[index].Value;
		}
		
		return 0;
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
