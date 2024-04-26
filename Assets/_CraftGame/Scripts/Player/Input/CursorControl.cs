using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControl : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private TilemapObject _floorTm;
	[SerializeField] private TilemapObject _wallTm;
	[SerializeField] private ItemParameter _damageMinParameter;
	[SerializeField] private ItemParameter _damageMaxParameter;
	[SerializeField] private ItemParameter _clickDistanceParameter;
	
	private PlayerInput _playerInput;
	private Clickable _currentClickable;
	private InventoryItem _focusItem = new();
	private int _damageMin;
	private int _damageMax;
	private int _clickDistance = 1;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.PrimaryAction.started += Hit;
		_playerInput.Player.Interact.started += TryToInteract;
		_playerInput.Enable();
		
		GameSignals.FOCUS_ITEM_UPDATED.AddListener(FocusInventoryItemUpdated);
		GameSignals.ON_UI_ACTIVATED.AddListener(DisableControl);
		GameSignals.ON_UI_UNACTIVED.AddListener(EnableControl);
	}
	
	private void OnDestroy()
	{
		_playerInput.Disable();
		
		GameSignals.FOCUS_ITEM_UPDATED.RemoveListener(FocusInventoryItemUpdated);
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
		_focusItem = (InventoryItem)parameters.GetParameter("FocusItem");
		
		int clickDistance = ExtractParameterValue(_clickDistanceParameter);
		_clickDistance = clickDistance > 0 ? clickDistance : 1;
	}
	
	private void Hit(InputAction.CallbackContext context)
	{
		if(Pointer.IsOverUI() || _focusItem.Item is not ToolObject) return;
		
		if(_currentClickable != null)
		{
			ToolObject tool = _focusItem.Item as ToolObject;
		
			_damageMin = ExtractParameterValue(_damageMinParameter);
			_damageMax = ExtractParameterValue(_damageMaxParameter);
			
			System.Random rnd = new System.Random();
			var damage = rnd.Next(_damageMin, _damageMax + 1); 
			
			_currentClickable.Hit(damage, tool.ToolType);
		}
		else
		{
			HitTilemap();
		}
	}
	
	private void HitTilemap()
	{
		var pos = Vector3Int.FloorToInt(transform.position);
		
		if(_wallTm.Tilemap.HasTile(pos))
			_wallTm.DynamicTilemap.Hit(pos, ToolType.Hammer);
		else if(_floorTm.Tilemap.HasTile(pos))
			_floorTm.DynamicTilemap.Hit(pos, ToolType.Hammer);
	}
	
	private int ExtractParameterValue(ItemParameter paramter)
	{
		if(_focusItem == null || _focusItem.Item == null) return 0;
		
		var itemParams = _focusItem.Item.DefaultParameterList;

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

		taPosition = Vector2.Distance(playerPos, _po.MousePosition) > _clickDistance ? (playerPos += new Vector2(0, 0.25f)) + (direction * _clickDistance) : _po.MousePosition;

		return taPosition;
	}
}