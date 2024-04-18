using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResearchController : MonoBehaviour
{
	private ResearchView _researchView;
	private PlayerInput _playerInput;
	
	private void Awake()
	{
		_playerInput = new();
		_playerInput.Player.ResearchMenu.started += ToggleResearchMenu;
		_playerInput.Enable();
	}
	
	private void OnDestroy()
	{
		_playerInput.Disable();
	}
	
	private void Start()
	{
		InitializeViews();
	}
	
	private void ToggleResearchMenu(InputAction.CallbackContext context)
	{
		_researchView.ToggleView();
	}
	
	private void InitializeViews()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_researchView = FindObjectOfType<ResearchView>();
	}
}
