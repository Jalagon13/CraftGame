using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedRecipeView : MonoBehaviour
{
	[SerializeField] private IngredientView _ingredientView;
	[SerializeField] private Transform _ingredientViewHolder;
	[SerializeField] private TextMeshProUGUI _menuTileText;
	
	private CraftingController _craftingController;
	
	public void Initialize(CraftingController craftingController)
	{
		_craftingController = craftingController;
		
		RenameMenuName(_craftingController.CraftingModel.SelectedRecipe.OutputItem.Name);
		
		// Clear all Ingredient Views
		foreach (Transform child in _ingredientViewHolder)
		{
			Destroy(child.gameObject);
		}
		
		// Populate Ingredient View Holder with Ingredients
		foreach (SelectedResource ingredient in _craftingController.CraftingModel.SelectedResources)
		{
			IngredientView ingredientView = Instantiate(_ingredientView, _ingredientViewHolder);
			ingredientView.Initialize(ingredient.Resource, ingredient.InventoryAmount, ingredient.RequiredAmount);
		}
	}
	
	private void RenameMenuName(string menuName)
	{
		_menuTileText.text = menuName;
	}
	
	public void CraftButton()
	{
		_craftingController.StartCrafting();
	}
	
	public void IncrementButton()
	{
		_craftingController.IncrementCraftAmount();
	}
	
	public void DecrementButton()
	{
		_craftingController.DecrementCraftAmount();
	}
	
	public void CancelCrafting()
	{
		_craftingController.CancelCrafting();
	}
}
