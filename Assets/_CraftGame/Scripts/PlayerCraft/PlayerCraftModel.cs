using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraftModel
{
	private List<PlayerRecipe> _playerRecipes;
	public List<PlayerRecipe> PlayerRecipes => _playerRecipes;
	
	public PlayerCraftModel(List<PlayerRecipe> playerRecipes)
	{
		_playerRecipes = playerRecipes;
	}
	
	public void DiscoverRecipe(ItemObject researchedItem)
	{
		foreach (PlayerRecipe recipe in _playerRecipes)
		{
			if(recipe.Recipe.OutputItem.Name == researchedItem.Name)
			{
				int index = _playerRecipes.IndexOf(recipe);
				_playerRecipes[index].Discovered = true;
				return;
			}
		}
		
		Debug.LogError($"Did not find Item: {researchedItem.Name} to unlock after research!");
	}
}
