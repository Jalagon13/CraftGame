using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
	[SerializeField] private Color _highlightedColor;
	
	private HotbarView _hotbarView;
	private List<InventorySlotView> _inventorySlotViews = new();
	private PlayerInput _playerInput;
	private Color _unHighlightedColor;
	private InventoryModel _playerInventory;
	private InventoryItem _focusInventoryItem;
	private int _selectedSlotIndex;
	
	public InventoryModel PlayerInventory { set { _playerInventory = value; } }
	
	private void Awake()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_hotbarView = FindObjectOfType<HotbarView>();
		
		_playerInput = new();
		_playerInput.Hotbar.Scroll.performed += SelectSlotScroll;
		_playerInput.Hotbar._1.started += SelectSlot;
		_playerInput.Hotbar._2.started += SelectSlot;
		_playerInput.Hotbar._3.started += SelectSlot;
		_playerInput.Hotbar._4.started += SelectSlot;
		_playerInput.Hotbar._5.started += SelectSlot;
		_playerInput.Hotbar._6.started += SelectSlot;
		_playerInput.Hotbar._7.started += SelectSlot;
		_playerInput.Hotbar._8.started += SelectSlot;
		_playerInput.Hotbar._9.started += SelectSlot;
	}
	
	private void OnEnable()
	{
		_playerInput.Enable();
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
	}
	
	private void Start()
	{
		foreach (Transform inventorySlotView in _hotbarView.transform)
		{
			var isv = inventorySlotView.GetComponent<InventorySlotView>();
			_inventorySlotViews.Add(isv);
		}
		
		_unHighlightedColor = _inventorySlotViews[0].GetComponent<Image>().color;
		
		HighlightSelectedSlot();
		GetFocusItem();
	}
	
	private void SelectSlot(InputAction.CallbackContext context)
	{
		_selectedSlotIndex = Int32.Parse(context.action.name) - 1;
		
		HighlightSelectedSlot();
		GetFocusItem();
	}
	
	private void SelectSlotScroll(InputAction.CallbackContext context)
	{
		if(Pointer.IsOverUI()) return;
		
		float scrollNum = context.ReadValue<float>();
		
		if (scrollNum < 0)
		{
			_selectedSlotIndex++;
			if (_selectedSlotIndex > _inventorySlotViews.Count - 1)
				_selectedSlotIndex = 0;
		}
		else if(scrollNum > 0)
		{
			_selectedSlotIndex--;
			if(_selectedSlotIndex < 0)
				_selectedSlotIndex = _inventorySlotViews.Count - 1;
		}
		
		HighlightSelectedSlot();
		GetFocusItem();
	}
	
	private void GetFocusItem()
	{
		_focusInventoryItem = _playerInventory.InventoryItems[_selectedSlotIndex];
		
		Signal signal = GameSignals.FOCUS_INVENTORY_ITEM_UPDATED;
		signal.ClearParameters();
		signal.AddParameter("FocusInventoryItem", _focusInventoryItem);
		signal.Dispatch();
	}
	
	private void HighlightSelectedSlot()
	{
		foreach (InventorySlotView isv in _inventorySlotViews)
		{
			isv.transform.GetComponent<Image>().color = _unHighlightedColor;
		}
		
		var selectedSlot = _hotbarView.transform.GetChild(_selectedSlotIndex);
		selectedSlot.GetComponent<Image>().color = _highlightedColor;
	}
}
