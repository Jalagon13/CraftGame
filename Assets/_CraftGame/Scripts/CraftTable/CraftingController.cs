using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	
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
		
		GameSignals.ON_UI_ACTIVATED.AddListener(ExtractCraftingModel);
		GameSignals.ON_CRAFT_NODE_CLICKED.AddListener(ExtractRecipeIndex);
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
		
		GameSignals.ON_UI_ACTIVATED.RemoveListener(ExtractCraftingModel);
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
	
	// [Button("Select Crafting Recipe (Only first one for now)")]
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
			_craftingModel.IncrementCraftAmount();
			_craftingView.RefreshSelectedRecipe(this);
		}
	}
	
	// [Button("Decrement Craft Amount")]
	public void DecrementCraftAmount()
	{
		if(_craftingModel.CanDecrementCraftAmount())
		{
			_craftingModel.DecrementCraftAmount();
			_craftingView.RefreshSelectedRecipe(this);
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
		TryToCloseUI();
	}
	
	public void TryToCloseUI()
	{
		if(_craftingView.UiActive)
		{
			_craftingView.UiActive = false;
			_craftingModel.OnCraftingDone -= CheckOneTimeCraftRecipes;
			GameSignals.ON_UI_UNACTIVED.Dispatch();
		}
	}
	
	private void ExtractCraftingModel(ISignalParameters parameters)
	{
		if(_craftingView.UiActive || !parameters.HasParameter("CraftingModel")) return;
		
		_craftingModel = (CraftingModel)parameters.GetParameter("CraftingModel");
		_craftingModel.OnCraftingDone += CheckOneTimeCraftRecipes;
		_craftingView.Initialize(_craftingModel.CraftingRecipes, _craftingModel.DiscardedOneTimeCraftRecipes, _craftingModel.DisplayName);
		
		if(_craftingModel.SelectedRecipe != null)
		{
			_craftingView.RefreshSelectedRecipe(this);
		}
		
		_craftingView.UiActive = true;
		
		if(_craftingModel.IsCrafting)
		{
			_craftingView.SetSelectedRecipeLayout();
		}
		else
		{
			_craftingView.SetInitialLayout();
		}
	}
	
	private void CheckOneTimeCraftRecipes()
	{
		if(_craftingModel.CurrentCraftingRecipe.OneTimeCraftable)
		{
			_craftingView.Initialize(_craftingModel.CraftingRecipes, _craftingModel.DiscardedOneTimeCraftRecipes, _craftingModel.DisplayName);
		}
		
		if(_craftingModel.IsCrafting)
		{
			_craftingView.SetSelectedRecipeLayout();
		}
		else
		{
			_craftingView.SetInitialLayout();
		}
	}
	
	private void InitializeView()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_craftingView = FindObjectOfType<CraftingView>();
	}
}
