using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedRecipeView : MonoBehaviour
{
	[SerializeField] private ProgressView _progressView;
	[SerializeField] private IngredientView _ingredientView;
	[SerializeField] private Transform _ingredientViewHolder;
	[SerializeField] private TextMeshProUGUI _menuTileText;
	[Header("Buttons")]
	[SerializeField] private Button _craftButton;
	[SerializeField] private Button _incrementButton;
	[SerializeField] private Button _decrementButton;
	
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
	
	public void UpdateViewInfo()
	{
		if(_craftingController == null) return;
		
		
		int craftAmount = _craftingController.CraftingModel.CraftAmount;
		int outputAmount = _craftingController.CraftingModel.SelectedRecipe.OutputAmount;
		
		_craftButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _craftingController.CraftingModel.SelectedRecipe.OneTimeCraftable ? 
		$"Craft" : $"Craft {craftAmount} (x{outputAmount * craftAmount})";
		
		_craftButton.interactable = _craftingController.CraftingModel.CanCraft();
		
		_incrementButton.gameObject.SetActive(!_craftingController.CraftingModel.SelectedRecipe.OneTimeCraftable);
		_decrementButton.gameObject.SetActive(!_craftingController.CraftingModel.SelectedRecipe.OneTimeCraftable);
		
		_incrementButton.interactable = _craftingController.CraftingModel.CanIncrementCraftAmount();
		_decrementButton.interactable = _craftingController.CraftingModel.CanDecrementCraftAmount();
	}
	
	private void RenameMenuName(string menuName)
	{
		_menuTileText.text = menuName;
	}
	
	public void CraftButton()
	{
		if(_craftingController.CraftingModel.IsCrafting)
		{
			CancelCrafting();
		}
		
		_craftingController.StartCrafting();
		
		if(_craftingController.CraftingModel.IsCrafting)
		{
			_progressView.StartProcessViewUI(_craftingController.CraftingModel);
		}
		
		UpdateViewInfo();
	}
	
	public void IncrementButton()
	{
		_craftingController.IncrementCraftAmount();
		
		UpdateViewInfo();
	}
	
	public void DecrementButton()
	{
		_craftingController.DecrementCraftAmount();
		
		UpdateViewInfo();
	}
	
	public void CancelCrafting()
	{
		_craftingController.CancelCrafting();
		_progressView.StopProcessViewUI();
		
		UpdateViewInfo();
	}
}
