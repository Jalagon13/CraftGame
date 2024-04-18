using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraftView : MonoBehaviour
{
	[SerializeField] private PlayerCraftNodeView _playerCraftNodeView;
	
	private List<PlayerRecipe> _playerRecipesToDisplay;
	
	public void Initialize(List<PlayerRecipe> playerRecipesToDisplay)
	{
		_playerRecipesToDisplay = playerRecipesToDisplay;
		
		// Clear all craft nodes
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		
		// Populate Craft Nodes
		foreach (PlayerRecipe recipe in _playerRecipesToDisplay)
		{
			PlayerCraftNodeView playerCraftNodeView = Instantiate(_playerCraftNodeView, transform);
			playerCraftNodeView.Initialize(recipe);
		}
	}
}
