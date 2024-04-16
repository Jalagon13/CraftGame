using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedResource
	{
		public ItemObject Resource;
		public int InventoryAmount;
		public int RequiredAmount;
		
		public SelectedResource(ItemObject resource, int inventoryAmount, int requiredAmount)
		{
			Resource = resource;
			InventoryAmount = inventoryAmount;
			RequiredAmount = requiredAmount;
		}
	}

public class CraftingModel : MonoBehaviour, IInteractable
{
	[SerializeField] private string _displayName;
	[SerializeField] private PlayerObject _po;
	[SerializeField] private List<CraftingRecipeObject> _craftingRecipes;
	
	private Timer _craftingTimer;
	private CraftingRecipeObject _selectedRecipe;
	private List<SelectedResource> _selectedResources;
	private int _craftAmount = 1;
	private int _craftCounter;
	private bool _canCraft;
	
	public CraftingRecipeObject SelectedRecipe => _selectedRecipe;
	public List<CraftingRecipeObject> CraftingRecipes => _craftingRecipes;
	public Timer CraftingTimer => _craftingTimer;
	public List<SelectedResource> SelectedResources => _selectedResources;
	public int CraftAmount { get { return _craftAmount; } set {_craftAmount = value; } }
	public string DisplayName => _displayName;
	
	private void Update()
	{
		if(_craftingTimer == null) return;
		
		if(_craftingTimer.RemainingSeconds > 0)
		{
			_craftingTimer.Tick(Time.deltaTime);
			if(_craftingTimer != null)
				Debug.Log($"Crafting Seconds Remaining: {_craftingTimer.RemainingSeconds}");
		}
	}
	
	// When I get back need to add debugs to everything and test it before starting the view
	public void StartCrafting()
	{
		Debug.Log("Crafting Started");
		
		_craftCounter = _craftAmount;
		_craftAmount = 1;
		
		// Assume I have all the resources needed
		// Take items from player's inventory for total crafting want
		TakeItemsFromPlayer();
		
		// Begin a crafting timer, at the end of it, produce one ouput, and keep repeating until craftCounter == 0;
		
		Debug.Log($"Selected Recipe Craft Timer: {_selectedRecipe.CraftingTimer}");
		_craftingTimer = new(_selectedRecipe.CraftingTimer);
		_craftingTimer.OnTimerEnd += CraftOneOutput;
	}
	
	private void TakeItemsFromPlayer()
	{
		Debug.Log("Items taken from player");
		
		for (int i = 0; i < _craftCounter; i++)
		{
			foreach (InventoryItem item in _selectedRecipe.ResourceList)
			{
				_po.PlayerInventory.InventoryModel.RemoveItem(item.Item, item.Quantity);
			}
		}
	}
	
	private void CraftOneOutput()
	{
		// Spawn one output here
		SpawnOneOutput();
		
		_craftingTimer.OnTimerEnd -= CraftOneOutput;
		_craftingTimer = null;
		
		if(_craftCounter > 0)
		{
			Debug.Log("Continuing to next crafting counter");
			
			_craftCounter--;
			_craftingTimer = new(_selectedRecipe.CraftingTimer);
			_craftingTimer.OnTimerEnd += CraftOneOutput;
		}
	}
	
	private void SpawnOneOutput()
	{
		InventoryItem output = new()
		{
			Item = _selectedRecipe.OutputItem,
			Quantity = _selectedRecipe.OutputAmount	
		};
		
		Debug.Log("Spawn One Output");
		GameManager.Instance.SpawnItem(transform.position, output);
		
		_craftCounter--;
	}
	
	public void CancelCrafting()
	{
		_craftingTimer.OnTimerEnd -= CraftOneOutput;
		_craftingTimer = null;
		_craftCounter = 1;
		
		// Returns items based on how many iterations have gone through and are left
		ReturnItemsToPlayer();
	}
	
	private void ReturnItemsToPlayer()
	{
		for (int i = 0; i < _craftCounter; i++)
		{
			foreach (InventoryItem item in _selectedRecipe.ResourceList)
			{
				_po.PlayerInventory.InventoryModel.AddItem(item);
			}
		}
		
		_craftCounter = 0;
	}
	
	public bool CanCraft()
	{
		UpdateSelectedResourceList();
		
		foreach (SelectedResource sr in _selectedResources)
		{
			if(sr.RequiredAmount > sr.InventoryAmount)
			{
				return false;
			}
		}
		
		return true;
	}
	
	public void SelectCraftingRecipe(CraftingRecipeObject recipe)
	{
		_selectedRecipe = recipe;
		_craftCounter = 0;
		_craftAmount = 1;
		
		UpdateSelectedResourceList();
	}
	
	private void UpdateSelectedResourceList()
	{
		_selectedResources = new();
		_selectedResources.Clear();
		foreach (InventoryItem resource in _selectedRecipe.ResourceList)
		{
			int inventoryAmount = _po.PlayerInventory.InventoryModel.GetAmount(resource.Item);
			SelectedResource rsc = new(resource.Item, inventoryAmount, resource.Quantity * _craftAmount);
			_selectedResources.Add(rsc);
		}
	}
	
	public void IncrementCraftAmount()
	{
		// Increment Craft Amount
		_craftAmount++;
		
		// Update Selected Resources
		UpdateSelectedResourceList();
	}
	
	public void DecrementCraftAmount()
	{
		// Decrement Craft Amount
		_craftAmount--;
		
		// Updated Selected Resources
		UpdateSelectedResourceList();
	}
	
	public bool CanIncrementCraftAmount()
	{
		foreach (SelectedResource sr in _selectedResources)
		{
			int inventoryAmount = sr.InventoryAmount;
			int requiredTestAmount = sr.RequiredAmount * (_craftAmount + 1);
			
			if(requiredTestAmount > inventoryAmount)
			{
				return false;
			}
		}
		
		return true;
	}
	
	public bool CanDecrementCraftAmount()
	{
		return _craftAmount > 1;
	}
	
	public void OnInteract()
	{
		Signal signal = GameSignals.ON_CRAFT_TABLE_INTERACT;
		signal.ClearParameters();
		signal.AddParameter("CraftingModel", this);
		signal.Dispatch();
	}
	
	#region Select Methods
	private bool _selected;
	private void Selected()
	{
		_selected = true;
	}
	
	private void UnSelected()
	{
		_selected = false;
	}
	
	
	private void OnMouseOver()
	{
		Selected();
	}

	private void OnMouseExit()
	{
		UnSelected();
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.TryGetComponent(out CursorControl cc))
		{
			Selected();
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.TryGetComponent(out CursorControl cc))
		{
			UnSelected();
		}
	}
	#endregion
}
