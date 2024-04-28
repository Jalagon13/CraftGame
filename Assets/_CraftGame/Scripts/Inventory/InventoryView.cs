using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
	[SerializeField] private InventorySlotView _inventorySlotView;
	[SerializeField] private Transform _hotbarSlotHolder;
	[SerializeField] private Transform _inventorySlotHolder;
	[SerializeField] private Transform _playerCraftViewHolder;
	[SerializeField] private AudioClip _inventoryEnabledClip;
	[SerializeField] private AudioClip _inventoryDisabledClip;
	private List<InventorySlotView> _inventorySlotViews = new();
	private bool _inventoryEnabled;
	
	public bool HotBarEnabled { get { return _hotbarSlotHolder.gameObject.activeInHierarchy; } 
		set 
		{ 
			_hotbarSlotHolder.gameObject.SetActive(value);
		} 
	}
	public bool InventoryEnabled { get { return _inventoryEnabled; } 
		set 
		{ 
			_inventoryEnabled = value; 
			_inventorySlotHolder.gameObject.SetActive(value);
			_playerCraftViewHolder.gameObject.SetActive(value);
			
			if(value)
				MMSoundManagerSoundPlayEvent.Trigger(_inventoryEnabledClip, MMSoundManager.MMSoundManagerTracks.UI, default);
			else
				MMSoundManagerSoundPlayEvent.Trigger(_inventoryDisabledClip, MMSoundManager.MMSoundManagerTracks.UI, default);

		} 
	}
	
	private void Start()
	{
		_inventoryEnabled = false;
		_inventorySlotHolder.gameObject.SetActive(_inventoryEnabled);
		_playerCraftViewHolder.gameObject.SetActive(_inventoryEnabled);
	}
	
	public void Initialize(List<InventoryItem> inventoryItems)
	{
		// For the first 9 inventory items, generate the slots as hotbar slots
		for (int i = 0; i < inventoryItems.Count; i++)
		{
			// If one of the first 9 slots, add it to _hotbarSlotHolder, else, add it to _inventorySlotView
			if(i < 9)
			{
				InitializeSlot(_hotbarSlotHolder, i);
			}
			else
			{
				InitializeSlot(_inventorySlotHolder, i);
			}
		}
	}
	
	private void InitializeSlot(Transform slotHolder, int inventoryIndex)
	{
		var inventorySlotView = Instantiate(_inventorySlotView, default, Quaternion.identity);
		inventorySlotView.transform.SetParent(slotHolder);
		inventorySlotView.Initialize(inventoryIndex);
		_inventorySlotViews.Add(inventorySlotView);
	}
	
	public void UpdateView(List<InventoryItem> updatedInventory)
	{
		int counter = 0;
		
		foreach (InventorySlotView isv in _inventorySlotViews)
		{
			isv.UpdateView(updatedInventory[counter]);
			counter++;
		}
	}
}
