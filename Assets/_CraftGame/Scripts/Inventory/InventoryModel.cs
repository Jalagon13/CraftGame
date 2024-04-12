using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
	public event Action<List<InventoryItem>> OnInventoryUpdate;
	private List<InventoryItem> _inventoryItems = new();
	private int _slotAmount;
	
	public List<InventoryItem> InventoryItems => _inventoryItems;
	
	public InventoryModel(int slotAmount)
	{
		_slotAmount = slotAmount;
		
		for (int i = 0; i < _slotAmount; ++i) 
		{
   			_inventoryItems.Add(new InventoryItem(){Item = null, Quantity = 0});
		}
	}
	
	public void AddItem(InventoryItem itemToAdd)
	{
		// Check if the item already exists in the inventory
		foreach (InventoryItem item in _inventoryItems) 
		{
			if(item.Item == null) continue;
			
			if(item.Item.Name == itemToAdd.Item.Name)
			{
				IncreaseItemQuantity(item, itemToAdd.Quantity);
				OnInventoryUpdate?.Invoke(_inventoryItems);
				return;
			}
		}
		
		// If Item cannot be found in inventory, check for first empty slot
		foreach (InventoryItem item in _inventoryItems) 
		{
			if(item.Item == null)
			{
				OverrideItem(item, itemToAdd);
				OnInventoryUpdate?.Invoke(_inventoryItems);
				return;
			}
		}
		
		// Inventory is full functionality
		
	}
	
	private void IncreaseItemQuantity(InventoryItem item, int amount)
	{
		item.Quantity += amount;
	}
	
	private void OverrideItem(InventoryItem itemToBeReplaced, InventoryItem replaceitem)
	{
		itemToBeReplaced.Item = replaceitem.Item;
		itemToBeReplaced.Quantity = replaceitem.Quantity;
	}
	
	public void RemoveItem()
	{
		
	}
}
