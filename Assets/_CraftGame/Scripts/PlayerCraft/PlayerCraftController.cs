using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerRecipe
{
	public CraftingRecipeObject Recipe;
	public bool Discovered;
	
	public PlayerRecipe(CraftingRecipeObject recipe, bool discovered)
	{
		Recipe = recipe;
		Discovered = discovered;
	}
}

public class PlayerCraftController : MonoBehaviour
{
	[SerializeField] private List<PlayerRecipe> _playerRecipes;
	
	private PlayerCraftModel _playerCraftModel;
	private PlayerCraftView _playerCraftView;
	
	private void Awake()
	{
		_playerCraftModel = new(_playerRecipes);
		GameSignals.ON_RECIPE_RESEARCHED.AddListener(OnRecipeResearched);
	}
	
	private void OnDestroy()
	{
		GameSignals.ON_RECIPE_RESEARCHED.RemoveListener(OnRecipeResearched);
	}
	
	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		InitializeView();
	}
	
	public void UpdateView()
	{
		_playerCraftView.Initialize(_playerCraftModel.PlayerRecipes);
	}
	
	private void OnRecipeResearched(ISignalParameters parameters)
	{
		ItemObject researchItem = (ItemObject)parameters.GetParameter("ResearchItem");
		
		_playerCraftModel.DiscoverRecipe(researchItem);
		_playerCraftView.Initialize(_playerCraftModel.PlayerRecipes);
	}
	
	private void InitializeView()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_playerCraftView = FindObjectOfType<PlayerCraftView>(true);
		_playerCraftView.Initialize(_playerCraftModel.PlayerRecipes);
	}
}
