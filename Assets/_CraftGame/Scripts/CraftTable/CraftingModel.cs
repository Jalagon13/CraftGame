using System;
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
	public event Action OnCraftingDone;
	public event Action OnIterationDone;
	
	[SerializeField] private string _displayName;
	[SerializeField] private PlayerObject _po;
	[SerializeField] private List<CraftingRecipeObject> _craftingRecipes;
	
	private Timer _craftingTimer;
	private CraftingRecipeObject _selectedRecipe;
	private CraftingRecipeObject _currentCraftingRecipe;
	private List<SelectedResource> _selectedResources;
	private List<CraftingRecipeObject> _discardedOneTimeCraftRecipes = new();
	private Canvas _selectedCanvas;
	private int _craftAmount = 1;
	private int _craftCounter;
	private bool _isCrafting;
	
	public CraftingRecipeObject SelectedRecipe => _selectedRecipe;
	public CraftingRecipeObject CurrentCraftingRecipe => _currentCraftingRecipe;
	public List<CraftingRecipeObject> CraftingRecipes => _craftingRecipes;
	public List<CraftingRecipeObject> DiscardedOneTimeCraftRecipes => _discardedOneTimeCraftRecipes;
	public Timer CraftingTimer => _craftingTimer;
	public List<SelectedResource> SelectedResources => _selectedResources;
	public int CraftCounter { get { return _craftCounter; } } // Counter for when actually crafting
	public int CraftAmount { get { return _craftAmount; } } // Counter for when before crafting
	public string DisplayName => _displayName;
	public bool IsCrafting => _isCrafting;
	
	private void Awake()
	{
		_selectedCanvas = transform.GetChild(1).GetComponent<Canvas>();
	}
	
	private void Update()
	{
		if(_craftingTimer == null) return;
		
		if(_craftingTimer.RemainingSeconds > 0)
		{
			_craftingTimer.Tick(Time.deltaTime);
		}
	}
	
	// When I get back need to add debugs to everything and test it before starting the view
	public void StartCrafting()
	{
		_craftCounter = _craftAmount;
		_currentCraftingRecipe = _selectedRecipe;
		_craftAmount = 1;
		_isCrafting = true;
		
		TakeItemsFromPlayer();
		UpdateSelectedResourceList();
		
		_craftingTimer = new(_selectedRecipe.CraftingTimer);
		_craftingTimer.OnTimerEnd += CraftOneOutput;
	}
	
	private void TakeItemsFromPlayer()
	{
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
		
		// If a Recipe is a one time craftable, put it in the discarded pile
		if(_currentCraftingRecipe.OneTimeCraftable)
		{
			_discardedOneTimeCraftRecipes.Add(_selectedRecipe);
		}
		
		if(_craftCounter > 0)
		{
			_craftingTimer.RemainingSeconds = _selectedRecipe.CraftingTimer;
			OnIterationDone?.Invoke();
		}
		else
		{
			_isCrafting = false;
			OnCraftingDone?.Invoke();
		}
	}
	
	private void SpawnOneOutput()
	{
		InventoryItem output = new()
		{
			Item = _selectedRecipe.OutputItem,
			Quantity = _selectedRecipe.OutputAmount	
		};
		
		GameManager.Instance.SpawnItem(transform.position + new Vector3(0.5f, 0.5f), output);
		
		_craftCounter--;
	}
	
	public void CancelCrafting()
	{
		// Returns items based on how many iterations have gone through and are left
		ReturnItemsToPlayer();
		
		_craftingTimer.OnTimerEnd -= CraftOneOutput;
		_craftCounter = 1;
		_craftAmount = 1;
		_isCrafting = false;
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
			SelectedResource rsc = new(resource.Item, inventoryAmount, resource.Quantity);
			_selectedResources.Add(rsc);
		}
	}
	
	public void IncrementCraftAmount()
	{
		_craftAmount++;
		
		UpdateSelectedResourceList();
	}
	
	public void DecrementCraftAmount()
	{
		_craftAmount--;
		
		UpdateSelectedResourceList();
	}
	
	public bool CanIncrementCraftAmount()
	{
		foreach (SelectedResource sr in _selectedResources)
		{
			int inventoryAmount = sr.InventoryAmount;
			int requiredTestAmount = sr.RequiredAmount * (_craftAmount + 1);
			Debug.Log($"Base Required Amount: {sr.RequiredAmount}");
			Debug.Log($"Required Test Amount: {requiredTestAmount}");
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
		Signal signal = GameSignals.ON_UI_ACTIVATED;
		signal.ClearParameters();
		signal.AddParameter("CraftingModel", this);
		signal.Dispatch();
	}
	
	#region Select Methods
	private bool _selected;
	private void Selected()
	{
		_selected = true;
		_selectedCanvas.gameObject.SetActive(true);
	}
	
	private void UnSelected()
	{
		_selected = false;
		_selectedCanvas.gameObject.SetActive(false);
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
