using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
	[SerializeField] private ItemObject _testItem; // delete later
	[SerializeField] private int _slotAmount;
	
	private InventoryModel _inventoryModel;
	private InventoryView _inventoryView;
	private PlayerInput _playerInput;
	
	private void Awake()
	{
		_inventoryModel = new(_slotAmount);
		
		_playerInput = new PlayerInput();
		_playerInput.Player.ToggleInventory.started += ToggleInventroy;
	}
	
	private void OnEnable()
	{
		_playerInput.Enable();
		_inventoryModel.OnInventoryUpdate += OnInventoryUpdate;
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
		_inventoryModel.OnInventoryUpdate -= OnInventoryUpdate;
	}
	
	private void Start()
	{
		InitializeInventoryView();
		CollectItem(new InventoryItem(){ Item = _testItem, Quantity = 11});
	}
	
	private void OnInventoryUpdate(List<InventoryItem> updatedInventory)
	{
		_inventoryView.UpdateInventoryView(updatedInventory);
	}
	
	private void InitializeInventoryView()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_inventoryView = FindObjectOfType<InventoryView>();
		_inventoryView.Initialize(_inventoryModel.InventoryItems);
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
