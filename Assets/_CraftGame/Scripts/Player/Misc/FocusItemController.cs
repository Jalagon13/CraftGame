using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocusItemController : MonoBehaviour
{
	private InventoryItem _focusItem;
	private PlayerInput _playerInput;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.SecondaryAction.started += ExecuteSecondaryAction;
		
		GameSignals.FOCUS_INVENTORY_ITEM_UPDATED.AddListener(OnFocusInventoryItemUpdated);
	}
	
	private void OnEnable()
	{
		_playerInput.Enable();
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
	}
	
	private void OnDestroy()
	{
		GameSignals.FOCUS_INVENTORY_ITEM_UPDATED.AddListener(OnFocusInventoryItemUpdated);
	}
	
	private void ExecuteSecondaryAction(InputAction.CallbackContext context)
	{
		if(_focusItem.Item == null || Pointer.IsOverUI()) return;
		
		_focusItem.Item.ExecuteSecondaryAction(this);
	}
	
	private void OnFocusInventoryItemUpdated(ISignalParameters parameters)
	{
		_focusItem = (InventoryItem)parameters.GetParameter("FocusInventoryItem");
	}
}
