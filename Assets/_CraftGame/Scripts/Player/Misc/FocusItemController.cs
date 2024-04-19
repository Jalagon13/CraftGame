using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocusItemController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private CursorControl _cursorControl;
	
	private InventoryItem _focusItem;
	private PlayerInput _playerInput;
	
	public CursorControl CursorControl => _cursorControl;	
	public InventoryItem FocusItem => _focusItem;
	public PlayerObject PlayerObject => _po;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.SecondaryAction.started += ExecuteSecondaryAction;
		_playerInput.Player.PrimaryAction.started += ExecutePrimaryAction;
		_playerInput.Enable();
		
		GameSignals.FOCUS_INVENTORY_ITEM_UPDATED.AddListener(OnFocusInventoryItemUpdated);
	}
	
	private void OnDestroy()
	{
		_playerInput.Disable();
		GameSignals.FOCUS_INVENTORY_ITEM_UPDATED.AddListener(OnFocusInventoryItemUpdated);
	}
	
	private void ExecutePrimaryAction(InputAction.CallbackContext context)
	{
		if(_focusItem.Item == null || Pointer.IsOverUI()) return;
		
		_focusItem.Item.ExecutePrimaryAction(this);
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
	
	public bool IsClear(Vector2 position)
	{
		Vector2 positionCheck = new(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
		var colliders = Physics2D.OverlapBoxAll(positionCheck + new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), 0);

		foreach(Collider2D col in colliders)
		{
			if(col.TryGetComponent(out Clickable clickable)) 
				return false;
		}

		return true;
	}
}
