using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedRecipeView : MonoBehaviour
{
	[SerializeField] private IngredientView _ingredientView;
	[SerializeField] private Transform _ingredientViewHolder;
	[SerializeField] private TextMeshProUGUI _menuTileText;
	
	private CraftingModel _craftingModel;
	
	public void Initialize(CraftingModel craftingModel)
	{
		_craftingModel = craftingModel;
		
		RenameMenuName(_craftingModel.SelectedRecipe.OutputItem.Name);
		
		// Clear all Ingredient Views
		foreach (Transform child in _ingredientViewHolder)
		{
			Destroy(child.gameObject);
		}
		
		// Populate Ingredient View Holder with Ingredients
		foreach (SelectedResource ingredient in _craftingModel.SelectedResources)
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
		
	}
	
	public void IncrementButton()
	{
		
	}
	
	public void DecrementButton()
	{
		
	}
}
