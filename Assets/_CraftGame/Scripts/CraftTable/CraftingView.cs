using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine; 
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftingView : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private CraftNodeView _craftNodeView;
	[SerializeField] private SelectedRecipeView _selectedRecipeView;
	[SerializeField] private Transform _craftNodeHolder;
	[SerializeField] private Transform _craftNodeHolderBackground;
	[SerializeField] private TextMeshProUGUI _menuTitleText;
	
	private List<CraftingRecipeObject> _recipesToDisplay;
	private List<CraftingRecipeObject> _discardedOneTimeCraftRecipes;
	private bool _uiActive;
	
	public bool UiActive { get { return _uiActive; } 
		set 
		{ 
			_uiActive = value;
			_po.SomeUiActive = value;
			
			LoopThroughChildElements(value);
			
			if(value)
			{
				SetInitialLayout();
			}
		} 
	}
	
	public void Initialize(List<CraftingRecipeObject> recipesToDisplay, List<CraftingRecipeObject> discardedOneTimeCraftRecipies, string craftTableName)
	{
		_recipesToDisplay = recipesToDisplay;
		_discardedOneTimeCraftRecipes = discardedOneTimeCraftRecipies;
		
		// Rename Menu Name
		RenameMenuName(craftTableName);
		
		// Clear all craft nodes
		foreach (Transform child in _craftNodeHolder)
		{
			Destroy(child.gameObject);
		}
		
		// Populate CraftNodeHolder with recipes
		foreach (CraftingRecipeObject recipe in _recipesToDisplay)
		{
			if(_discardedOneTimeCraftRecipes.Contains(recipe)) continue;
			
			CraftNodeView craftNodeView = Instantiate(_craftNodeView, _craftNodeHolder);
			craftNodeView.Initialize(recipe.OutputItem, _recipesToDisplay.IndexOf(recipe));
		}
	}
	
	public void SetInitialLayout()
	{
		// Crafting SelectedRecipeView Disabled
		_selectedRecipeView.UpdateViewInfo();
		_selectedRecipeView.gameObject.SetActive(false);
		// Crafting NodeHolder Enabled and Centered
		// _craftNodeHolderBackground
	}
	
	public void SetSelectedRecipeLayout()
	{
		// Crafting NodeHolder Enabled but moved to the left
		
		// Crafting SelectedRecipeView Enabled
		_selectedRecipeView.UpdateViewInfo();
		_selectedRecipeView.gameObject.SetActive(true);
	}
	
	private void RenameMenuName(string craftTableName)
	{
		_menuTitleText.text = craftTableName;
		// Add sprite next to this text later on
	}
	
	public void RefreshSelectedRecipe(CraftingController craftingController)
	{
		_selectedRecipeView.Initialize(craftingController);
		SetSelectedRecipeLayout(); 
	}
	
	private void LoopThroughChildElements(bool enabled)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform childTransform = transform.GetChild(i);
			childTransform.gameObject.SetActive(enabled);
		}
	}
}
