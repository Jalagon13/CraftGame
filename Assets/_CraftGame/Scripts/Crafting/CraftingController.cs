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
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.Interact.started += TryToCloseUI;
		_playerInput.Player.Esc.started += TryToCloseUI;
		_playerInput.Enable();
		
		GameSignals.ON_CRAFT_TABLE_INTERACT.AddListener(ExtractCraftingModel);
	}
	
	private void OnDisable()
	{
		_playerInput.Disable();
		
		GameSignals.ON_CRAFT_TABLE_INTERACT.RemoveListener(ExtractCraftingModel);
	}
	
	private void Start()
	{
		InitializeView();
		_craftingView.UiActive = false;
	}
	
	[Button("Select Crafting Recipe (Only first one for now)")]
	public void SelectCraftingRecipe()
	{
		_craftingModel.SelectCraftingRecipe(_craftingModel.CraftingRecipes[0]); // Testing only the first element for now
	}
	
	[Button("Start Crafting")]
	public void StartCrafting()
	{
		if(_craftingModel.CanCraft())
		{
			_craftingModel.StartCrafting();
		}
	}
	
	[Button("Increment Craft Amount")]
	public void IncrementCraftAmount()
	{
		if(_craftingModel.CanIncrementCraftAmount())
		{
			Debug.Log("Can Increment, Incremented Craft Amount");
			_craftingModel.IncrementCraftAmount();
		}
		else
		{
			Debug.Log("Can NOT Increment");
		}
	}
	
	[Button("Decrement Craft Amount")]
	public void DecrementCraftAmount()
	{
		if(_craftingModel.CanDecrementCraftAmount())
		{
			Debug.Log("Can Decrement, Decremented Craft Amount");
			_craftingModel.DecrementCraftAmount();
		}
		else
		{
			Debug.Log("Can NOT Decrement");
		}
	}
	
	[Button("Cancel Crafting")]
	public void CancelCrafting()
	{
		_craftingModel.CancelCrafting();
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
		// refresh UI display here
		_craftingView.UiActive = true;
	}
	
	private void InitializeView()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_craftingView = FindObjectOfType<CraftingView>();
	}
}
