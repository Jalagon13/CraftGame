using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private int _slotAmount;
	[SerializeField] private MMF_Player _slotClickFeedbacks;
	[SerializeField] private UnityEvent _onInventoryUpdate;
	[SerializeField] private UnityEvent _onInventoryOpen;
	[SerializeField] private List<InventoryItem> _startingItems = new();
	
	private InventoryModel _inventoryModel;
	private InventoryView _inventoryView;
	private MouseItemModel _mouseItemModel;
	private MouseItemView _mouseItemView;
	private HotbarController _hotbarController;
	private PlayerInput _playerInput;
	private bool _gotItemThisFrame, _gaveItemThisFrame;
	
	public InventoryModel InventoryModel => _inventoryModel;
	public MouseItemModel MouseItem => _mouseItemModel;
	public MouseItemView MouseItemView => _mouseItemView;
	
	private void Awake()
	{
		_mouseItemModel = new();
		_inventoryModel = new(_slotAmount, _mouseItemModel.MouseInventoryItem);
		_hotbarController = GetComponent<HotbarController>();
		_hotbarController.PlayerInventory = _inventoryModel;
		
		_playerInput = new PlayerInput();
		_playerInput.Player.ToggleInventory.started += ToggleInventroy;
		
		_po.PlayerInventory = this;
	}
	
	private void OnEnable()
	{
		_playerInput.Enable();
		_inventoryModel.OnInventoryUpdate += OnInventoryUpdate;
		
		GameSignals.ON_SLOT_LEFT_CLICKED.AddListener(InventorySlotLeftClicked);
		GameSignals.ON_SLOT_RIGHT_CLICKED.AddListener(InventorySlotRightClicked);
		GameSignals.ON_UI_ACTIVATED.AddListener(DisableControl);
		GameSignals.ON_UI_UNACTIVED.AddListener(EnableControl);
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
		_inventoryModel.OnInventoryUpdate -= OnInventoryUpdate;
		
		GameSignals.ON_SLOT_LEFT_CLICKED.RemoveListener(InventorySlotLeftClicked);
		GameSignals.ON_SLOT_RIGHT_CLICKED.RemoveListener(InventorySlotRightClicked);
		GameSignals.ON_UI_ACTIVATED.RemoveListener(DisableControl);
		GameSignals.ON_UI_UNACTIVED.RemoveListener(EnableControl);
	}
	
	private IEnumerator Start()
	{
		InitializeViews();
		
		yield return new WaitForEndOfFrame();
		
		foreach(InventoryItem item in _startingItems)
		{
			CollectItem(item);
		}
	}
	
	private void Update()
	{
		DispatchHandle();
	}
	
	// note refactor this later 
	private void DispatchHandle()
	{
		if (_mouseItemModel.MouseInventoryItem.HasItem())
		{
			if (_gotItemThisFrame) return;

			Signal signal = GameSignals.MOUSE_GOT_ITEM;
			signal.ClearParameters();
			signal.AddParameter("MouseItem", _mouseItemModel.MouseInventoryItem);
			signal.Dispatch();
			_gotItemThisFrame = true;
			_gaveItemThisFrame = false;
		}
		else
		{
			if (_gaveItemThisFrame) return;

			GameSignals.MOUSE_GAVE_ITEM.Dispatch();
			_gaveItemThisFrame = true;
			_gotItemThisFrame = false;
		}
	}
	
	private void InventorySlotRightClicked(ISignalParameters parameters)
	{
		int clickedInventorySlotIndex = (int)parameters.GetParameter("inventoryIndex");
		InventoryItem inventoryItem = _inventoryModel.InventoryItems[clickedInventorySlotIndex];
		InventoryItem mouseItem = _mouseItemModel.MouseInventoryItem;
		
		if(inventoryItem.HasItem())
		{
			if(mouseItem.HasItem())
			{
				if(inventoryItem.Item.Name == mouseItem.Item.Name)
				{
					inventoryItem.Quantity += mouseItem.Quantity;
					mouseItem.Item = null;
					mouseItem.Quantity = 0;
					PlayClickFeedbacks();
				}
				else
				{
					// Swap the two items
					ItemObject tempItem = inventoryItem.Item;
					int tempQuantity = inventoryItem.Quantity;
					
					inventoryItem.Item = mouseItem.Item;
					inventoryItem.Quantity = mouseItem.Quantity;
					
					mouseItem.Item = tempItem;
					mouseItem.Quantity = tempQuantity;
					PlayClickFeedbacks();
				}
			}
			else
			{
				int inventoryItemQuantity = inventoryItem.Quantity;
				int newInventoryItemQuantity = inventoryItemQuantity / 2;
				int newMouseItemQuantity = inventoryItemQuantity - newInventoryItemQuantity;
				
				inventoryItem.Quantity = newInventoryItemQuantity;
				
				mouseItem.Item = inventoryItem.Item;
				mouseItem.Quantity = newMouseItemQuantity;
				
				if(inventoryItem.Quantity == 0)
				{
					inventoryItem.Item = null;
				}
				PlayClickFeedbacks();
			}
		}
		else
		{
			if(mouseItem.HasItem())
			{
				
			}
		}
		
		// update views
		_inventoryView.UpdateView(_inventoryModel.InventoryItems);
		_mouseItemView.UpdateView(_mouseItemModel.MouseInventoryItem);
	}
	
	private void InventorySlotLeftClicked(ISignalParameters parameters)
	{
		int clickedInventorySlotIndex = (int)parameters.GetParameter("inventoryIndex");
		InventoryItem inventoryItem = _inventoryModel.InventoryItems[clickedInventorySlotIndex];
		InventoryItem mouseItem = _mouseItemModel.MouseInventoryItem;
		
		if(inventoryItem.HasItem())
		{
			if(mouseItem.HasItem())
			{
				if(inventoryItem.Item.Name == mouseItem.Item.Name)
				{
					inventoryItem.Quantity += mouseItem.Quantity;
					mouseItem.Item = null;
					mouseItem.Quantity = 0;
					PlayClickFeedbacks();
				}
				else
				{
					// Swap the two items
					ItemObject tempItem = inventoryItem.Item;
					int tempQuantity = inventoryItem.Quantity;
					
					inventoryItem.Item = mouseItem.Item;
					inventoryItem.Quantity = mouseItem.Quantity;
					
					mouseItem.Item = tempItem;
					mouseItem.Quantity = tempQuantity;
					PlayClickFeedbacks();
				}
			}
			else
			{
				mouseItem.Item = inventoryItem.Item;
				mouseItem.Quantity = inventoryItem.Quantity;
				
				inventoryItem.Item = null;
				inventoryItem.Quantity = 0;
				PlayClickFeedbacks();
			}
		}
		else
		{
			if(mouseItem.HasItem())
			{
				inventoryItem.Item = mouseItem.Item;
				inventoryItem.Quantity = mouseItem.Quantity;
				
				mouseItem.Item = null;
				mouseItem.Quantity = 0;
				PlayClickFeedbacks();
			}
		}
		
		// update views
		_inventoryView.UpdateView(_inventoryModel.InventoryItems);
		_mouseItemView.UpdateView(_mouseItemModel.MouseInventoryItem);
	}
	
	private void PlayClickFeedbacks()
	{
		_slotClickFeedbacks?.PlayFeedbacks();
	}
	
	private void OnInventoryUpdate(List<InventoryItem> updatedInventory)
	{
		_inventoryView.UpdateView(updatedInventory);
	}
	
	private void InitializeViews()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_inventoryView = FindObjectOfType<InventoryView>();
		_inventoryView.Initialize(_inventoryModel.InventoryItems);
		_mouseItemView = FindObjectOfType<MouseItemView>();
		_mouseItemView.Initialize();
	}
	
	private void EnableControl(ISignalParameters parameters)
	{
		_inventoryView.HotBarEnabled = true;
		_playerInput.Enable();
	}
	
	private void DisableControl(ISignalParameters parameters)
	{
		_inventoryView.InventoryEnabled = false;
		_inventoryView.HotBarEnabled = false;
		_playerInput.Disable();
	}
	
	private void ToggleInventroy(InputAction.CallbackContext context)
	{
		_inventoryView.InventoryEnabled = !_inventoryView.InventoryEnabled;
		
		if(_inventoryView.InventoryEnabled)
		{
			_onInventoryOpen?.Invoke();	
		}
	}
	
	public void CollectItem(InventoryItem itemToCollect)
	{
		_inventoryModel.AddItem(itemToCollect);
		_onInventoryUpdate?.Invoke();
		
		_inventoryView.UpdateView(_inventoryModel.InventoryItems);
		_mouseItemView.UpdateView(_mouseItemModel.MouseInventoryItem);
	}
	
	public void RemoveItem(ItemObject itemToRemove, int quantity)
	{
		_inventoryModel.RemoveItem(itemToRemove, quantity);
		_onInventoryUpdate?.Invoke();
		
		_inventoryView.UpdateView(_inventoryModel.InventoryItems);
		_mouseItemView.UpdateView(_mouseItemModel.MouseInventoryItem);
	}
}
