using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingController : MonoBehaviour
{
	private PlayerInput _playerInput;
	private CraftingModel _craftingModel;
	private CraftingView _craftingView;
	
	public CraftingModel CraftingModel => _craftingModel;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.Interact.started += TryToCloseUI;
		_playerInput.Player.Esc.started += TryToCloseUI;
		_playerInput.Enable();
		
		GameSignals.ON_CRAFT_TABLE_INTERACT.AddListener(ExtractCraftingModel);
		GameSignals.ON_CRAFT_NODE_CLICKED.AddListener(ExtractRecipeIndex);
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
		
		GameSignals.ON_CRAFT_TABLE_INTERACT.RemoveListener(ExtractCraftingModel);
		GameSignals.ON_CRAFT_NODE_CLICKED.RemoveListener(ExtractRecipeIndex);
	}
	
	private void Start()
	{
		InitializeView();
		_craftingView.UiActive = false;
	}
	
	private void ExtractRecipeIndex(ISignalParameters parameters)
	{
		int recipeIndex = (int)parameters.GetParameter("RecipeIndex");
		
		SelectCraftingRecipe(recipeIndex);
		_craftingView.RefreshSelectedRecipe(this);
	}
	
	[Button("Select Crafting Recipe (Only first one for now)")]
	public void SelectCraftingRecipe(int index)
	{
		_craftingModel.SelectCraftingRecipe(_craftingModel.CraftingRecipes[index]); // Testing only the first element for now
		_craftingView.RefreshSelectedRecipe(this);
	}
	
	// [Button("Start Crafting")]
	public void StartCrafting()
	{
		if(_craftingModel.CanCraft())
		{
			_craftingModel.StartCrafting();
			_craftingView.RefreshSelectedRecipe(this);
		}
	}
	
	// [Button("Increment Craft Amount")]
	public void IncrementCraftAmount()
	{
		if(_craftingModel.CanIncrementCraftAmount())
		{
			Debug.Log("Can Increment, Incremented Craft Amount");
			_craftingModel.IncrementCraftAmount();
			_craftingView.RefreshSelectedRecipe(this);
		}
		else
		{
			Debug.Log("Can NOT Increment");
		}
	}
	
	// [Button("Decrement Craft Amount")]
	public void DecrementCraftAmount()
	{
		if(_craftingModel.CanDecrementCraftAmount())
		{
			Debug.Log("Can Decrement, Decremented Craft Amount");
			_craftingModel.DecrementCraftAmount();
			_craftingView.RefreshSelectedRecipe(this);
		}
		else
		{
			Debug.Log("Can NOT Decrement");
		}
	}
	
	// [Button("Cancel Crafting")]
	public void CancelCrafting()
	{
		_craftingModel.CancelCrafting();
		_craftingView.RefreshSelectedRecipe(this);
	}
	
	private void TryToCloseUI(InputAction.CallbackContext context)
	{
		if(_craftingView.UiActive)
		{
			_craftingView.UiActive = false;
			_craftingModel = null;
			GameSignals.ON_CRAFT_TABLE_UNINTERACT.Dispatch();
		}
	}
	
	private void ExtractCraftingModel(ISignalParameters parameters)
	{
		if(_craftingView.UiActive) return;
		
		_craftingModel = (CraftingModel)parameters.GetParameter("CraftingModel");
		_craftingView.Initialize(_craftingModel.CraftingRecipes, _craftingModel.DisplayName);
		_craftingView.UiActive = true;
	}
	
	private void InitializeView()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_craftingView = FindObjectOfType<CraftingView>();
		
	}
}
