using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocusItemController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private TilemapObject _wallTm, _floorTm, _spawnFloorTilemap;
	[SerializeField] private CursorControl _cursorControl;
	
	private InventoryItem _focusItem, _hotbarItem, _mouseItem;
	private PlayerInput _playerInput;
	private bool _mouseSlotHasitem;
	
	public CursorControl CursorControl => _cursorControl;	
	public InventoryItem FocusItem => _focusItem;
	public PlayerObject PlayerObject => _po;
	public TilemapObject SpawnFloorTilemap => _spawnFloorTilemap;
	public TilemapObject WallTm { get { return _wallTm; } }
	public TilemapObject FloorTm { get { return _floorTm; } }
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.SecondaryAction.started += ExecuteSecondaryAction;
		_playerInput.Player.PrimaryAction.started += ExecutePrimaryAction;
		_playerInput.Enable();
		
		GameSignals.HOTBAT_SLOT_UPDATED.AddListener(OnHotbarItemUpdated);
		GameSignals.MOUSE_GOT_ITEM.AddListener(OnMouseGotItem);
		GameSignals.MOUSE_GAVE_ITEM.AddListener(OnMouseGaveItem);
	}
	
	private void OnDestroy()
	{
		_playerInput.Disable();
		
		GameSignals.HOTBAT_SLOT_UPDATED.RemoveListener(OnHotbarItemUpdated);
		GameSignals.MOUSE_GOT_ITEM.RemoveListener(OnMouseGotItem);
		GameSignals.MOUSE_GAVE_ITEM.RemoveListener(OnMouseGaveItem);
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
	
	private void OnHotbarItemUpdated(ISignalParameters parameters)
	{
		_hotbarItem = (InventoryItem)parameters.GetParameter("HotbarInventoryItem");
		
		SetFocusItem();
	}
	
	private void OnMouseGotItem(ISignalParameters parameters)
	{
		if (parameters.HasParameter("MouseItem"))
		{
			_mouseItem = (InventoryItem)parameters.GetParameter("MouseItem");
			_mouseSlotHasitem = true;
			SetFocusItem();
		}
	}
	
	private void OnMouseGaveItem(ISignalParameters parameters)
	{
		_mouseSlotHasitem = false;

		SetFocusItem();
	}
	
	private void SetFocusItem()
	{
		_focusItem = _mouseSlotHasitem ? _mouseItem : _hotbarItem;

		Signal signal = GameSignals.FOCUS_ITEM_UPDATED;
		signal.ClearParameters();
		signal.AddParameter("FocusItem", _focusItem);
		signal.Dispatch();
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
	
	public bool InValidHouse()
	{
		// Do this after you have the NPC walking
		
		return true;
	}
	
	public void RemoveItem()
	{
		
	}
}
