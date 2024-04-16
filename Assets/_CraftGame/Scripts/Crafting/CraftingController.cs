using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	
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
	
	private void TryToCloseUI(InputAction.CallbackContext context)
	{
		if(_craftingView.UiActive)
		{
			_craftingView.UiActive = false;
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
