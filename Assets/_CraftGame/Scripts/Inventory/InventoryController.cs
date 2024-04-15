using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private int _slotAmount;
	[SerializeField] private List<InventoryItem> _startingItems = new();
	
	private InventoryModel _inventoryModel;
	private InventoryView _inventoryView;
	private MouseItemModel _mouseItemModel;
	private MouseItemView _mouseItemView;
	private HotbarController _hotbarController;
	private PlayerInput _playerInput;
	
	private void Awake()
	{
		_inventoryModel = new(_slotAmount);
		_mouseItemModel = new();
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
		GameSignals.ON_CRAFT_TABLE_INTERACT.AddListener(DisableControl);
		GameSignals.ON_CRAFT_TABLE_UNINTERACT.AddListener(EnableControl);
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
		_inventoryModel.OnInventoryUpdate -= OnInventoryUpdate;
		
		GameSignals.ON_SLOT_LEFT_CLICKED.RemoveListener(InventorySlotLeftClicked);
		GameSignals.ON_SLOT_RIGHT_CLICKED.RemoveListener(InventorySlotRightClicked);
		GameSignals.ON_CRAFT_TABLE_INTERACT.RemoveListener(DisableControl);
		GameSignals.ON_CRAFT_TABLE_UNINTERACT.RemoveListener(EnableControl);
	}
	
	private void Start()
	{
		InitializeViews();
		
		foreach(InventoryItem item in _startingItems)
		{
			CollectItem(item);
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
				}
			}
			else
			{
				mouseItem.Item = inventoryItem.Item;
				mouseItem.Quantity = inventoryItem.Quantity;
				
				inventoryItem.Item = null;
				inventoryItem.Quantity = 0;
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
			}
		}
		
		// update views
		_inventoryView.UpdateView(_inventoryModel.InventoryItems);
		_mouseItemView.UpdateView(_mouseItemModel.MouseInventoryItem);
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
	}
	
	public void CollectItem(InventoryItem itemToCollect)
	{
		_inventoryModel.AddItem(itemToCollect);
	}
	
	public void RemoveItem(ItemObject itemToRemove, int quantity)
	{
		_inventoryModel.RemoveItem(itemToRemove, quantity);
	}
}
