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
			_inventoryItems.Add(new InventoryItem() { Item = null, Quantity = 0 });
		}
	}

	public void AddItem(InventoryItem itemToAdd)
	{
		// Check if the item already exists in the inventory
		foreach (InventoryItem item in _inventoryItems)
		{
			if (item.Item == null) continue;

			if (item.Item.Name == itemToAdd.Item.Name)
			{
				IncreaseItemQuantity(item, itemToAdd.Quantity);
				OnInventoryUpdate?.Invoke(_inventoryItems);
				return;
			}
		}

		// If Item cannot be found in inventory, check for first empty slot
		foreach (InventoryItem item in _inventoryItems)
		{
			if (item.Item == null)
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
	
	public void RemoveItem(ItemObject itemToRemove, int amountToRemove)
	{
		// Basic funationalty, need to revisit later to fix bugs
		foreach (InventoryItem item in _inventoryItems)
		{
			if(item.Item == null) continue;
			
			if(item.Item.Name == itemToRemove.Name)
			{
				item.Quantity -= amountToRemove;
				
				if(item.Quantity <= 0)
				{
					// Note to future self: BUG: You are able to remove an amount of items even if it is greater than what it is in the stack. Need to fix this later
					
					item.Item = null;
					item.Quantity = 0;
				}
				
				OnInventoryUpdate?.Invoke(_inventoryItems);
				return;
			}
		}
	}
	
	public bool Contains(InventoryItem inventoryItemToCheck)
	{
		int amountCounter = 0;
		
		foreach (InventoryItem item in _inventoryItems)
		{
			if(item.Item.Name == inventoryItemToCheck.Item.Name)
			{
				amountCounter += item.Quantity;
			}
		}
		
		return amountCounter >= inventoryItemToCheck.Quantity;
	}
	
	public int GetAmount(ItemObject itemToCheck)
	{
		int amountCounter = 0;
		
		foreach (InventoryItem item in _inventoryItems)
		{
			if(item.Item == null) continue;
			
			if(item.Item.Name == itemToCheck.Name)
			{
				amountCounter += item.Quantity;
			}
		}
		
		return amountCounter;
	}
}
