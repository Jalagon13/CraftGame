using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
	[SerializeField] private ItemObject _testItem; // Test delete later
	[SerializeField] private int _slotAmount;
	
	private InventoryModel _inventoryModel;
	private InventoryView _inventoryView;
	private MouseItemModel _mouseItemModel;
	private MouseItemView _mouseItemView;
	private PlayerInput _playerInput;
	
	private void Awake()
	{
		_inventoryModel = new(_slotAmount);
		_mouseItemModel = new();
		
		_playerInput = new PlayerInput();
		_playerInput.Player.ToggleInventory.started += ToggleInventroy;
	}
	
	private void OnEnable()
	{
		_playerInput.Enable();
		_inventoryModel.OnInventoryUpdate += OnInventoryUpdate;
		
		GameSignals.ON_SLOT_VIEW_CLICKED.AddListener(InventorySlotClicked);
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
		_inventoryModel.OnInventoryUpdate -= OnInventoryUpdate;
		
		GameSignals.ON_SLOT_VIEW_CLICKED.RemoveListener(InventorySlotClicked);
	}
	
	private void Start()
	{
		InitializeViews();
		CollectItem(new InventoryItem(){ Item = _testItem, Quantity = 11}); // Test delete later
	}
	
	private void InventorySlotClicked(ISignalParameters parameters)
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
					// Add Mouse Item to Inventory slot
					inventoryItem.Quantity += mouseItem.Quantity;
					mouseItem.Item = null;
					mouseItem.Quantity = 0;
				}
				else
				{
					// Swap the two items
				}
			}
			else
			{
				mouseItem.Item = inventoryItem.Item;
				mouseItem.Quantity = inventoryItem.Quantity;
			}
		}
		
		// update views
		// next up find a way to update views of everything for testing
	}
	
	private void OnInventoryUpdate(List<InventoryItem> updatedInventory)
	{
		_inventoryView.UpdateInventoryView(updatedInventory);
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
