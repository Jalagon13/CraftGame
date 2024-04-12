using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private ItemObject _testItem1; // Test delete later
	[SerializeField] private ItemObject _testItem2; // Test delete later
	[SerializeField] private int _slotAmount;
	
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
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
		_inventoryModel.OnInventoryUpdate -= OnInventoryUpdate;
		
		GameSignals.ON_SLOT_LEFT_CLICKED.RemoveListener(InventorySlotLeftClicked);
		GameSignals.ON_SLOT_RIGHT_CLICKED.RemoveListener(InventorySlotRightClicked);
	}
	
	private void Start()
	{
		InitializeViews();
		CollectItem(new InventoryItem(){ Item = _testItem1, Quantity = 11}); // Test delete later
		CollectItem(new InventoryItem(){ Item = _testItem2, Quantity = 9}); // Test delete later
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
	
	private void ToggleInventroy(InputAction.CallbackContext context)
	{
		_inventoryView.ToggleInventory();
	}
	
	public void CollectItem(InventoryItem itemToCollect)
	{
		_inventoryModel.AddItem(itemToCollect);
	}
}
