using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
	[SerializeField] private InventorySlotView _inventorySlotView;
	[SerializeField] private Transform _hotbarSlotHolder;
	[SerializeField] private Transform _inventorySlotHolder;
	private List<InventorySlotView> _inventorySlotViews = new();
	private bool _inventoryEnabled;
	
	private void Start()
	{
		_inventoryEnabled = false;
		_inventorySlotHolder.gameObject.SetActive(_inventoryEnabled);
	}
	
	public void Initialize(List<InventoryItem> inventoryItems)
	{
		// For the first 9 inventory items, generate the slots as hotbar slots
		for (int i = 0; i < inventoryItems.Count; i++)
		{
			// If one of the first 9 slots, add it to _hotbarSlotHolder, else, add it to _inventorySlotView
			if(i < 9)
			{
				InitializeSlot(_hotbarSlotHolder, inventoryItems[i]);
			}
			else
			{
				InitializeSlot(_inventorySlotHolder, inventoryItems[i]);
			}
		}
	}
	
	private void InitializeSlot(Transform slotHolder, InventoryItem item)
	{
		var inventorySlotView = Instantiate(_inventorySlotView, default, Quaternion.identity);
		inventorySlotView.transform.SetParent(slotHolder);
		inventorySlotView.UpdateView(item);
		_inventorySlotViews.Add(inventorySlotView);
	}
	
	public void UpdateInventoryView(List<InventoryItem> updatedInventory)
	{
		int counter = 0;
		
		foreach (InventorySlotView isv in _inventorySlotViews)
		{
			isv.UpdateView(updatedInventory[counter]);
			counter++;
		}
	}
	
	public void ToggleInventory()
	{
		_inventoryEnabled = !_inventoryEnabled;
		_inventorySlotHolder.gameObject.SetActive(_inventoryEnabled);
	}
	
	
}
