using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingModel : MonoBehaviour, IInteractable
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private List<CraftingRecipeObject> _craftingRecipes;
	
	private Timer _craftingTimer;
	private CraftingRecipeObject _selectedRecipe;
	private List<SelectedResource> _selectedResources;
	private int _craftAmount = 1;
	private bool _canCraft;
	
	public List<CraftingRecipeObject> CraftingRecipes => _craftingRecipes;
	
	private class SelectedResource
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
	
	public void StartCrafting()
	{
		
	}
	
	public void CancelCrafting()
	{
		
	}
	
	public bool CanCraft()
	{
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
		_selectedResources.Clear();
		_craftAmount = 1;
		
		// Populate Selected Resource List
		foreach (InventoryItem resource in recipe.ResourceList)
		{
			int inventoryAmount = _po.PlayerInventory.InventoryModel.GetAmount(resource.Item);
			SelectedResource rsc = new(resource.Item, inventoryAmount, resource.Quantity * _craftAmount);
			_selectedResources.Add(rsc);
		}
	}
	
	public void IncrementCraftAmount()
	{
		_craftAmount++;
	}
	
	public bool CanIncrementCraftAmount()
	{
		
		
		return true;
	}
	
	public void DecrementCraftAmount()
	{
		_craftAmount--;
	}
	
	public bool CanDecrementCraftAmount()
	{
		return true;
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
