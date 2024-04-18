using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCraftNodeView : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private Transform _ingredientViewHolder;
	[SerializeField] private PlayerIngredientView _playerIngredientView;
	[SerializeField] private TextMeshProUGUI _outputText;
	
	private PlayerRecipe _recipe;
	private Image _background;
	private Image _outputImage;
	public bool Discovered { get { return _recipe.Discovered;} }
	
	public void Initialize(PlayerRecipe recipe)
	{
		_recipe = recipe;
		_background = GetComponent<Image>();
		_outputImage = transform.GetChild(0).GetComponent<Image>();
		_outputImage.sprite = _recipe.Recipe.OutputItem.UiDisplay;
		_outputText.text = _recipe.Recipe.OutputItem.Name;
		
		if(_recipe.Discovered)
			SetDiscoveredState();
		else
			SetNotDiscoveredState();
		
		UpdateIngredientView();
	}
	
	private void UpdateIngredientView()
	{
		// Clear Ingredients in View Holder
		foreach (Transform child in _ingredientViewHolder)
		{
			Destroy(child.gameObject);
		}
		
		// Populate Ingredient View Holder
		foreach (InventoryItem ingredient in _recipe.Recipe.ResourceList)
		{
			PlayerIngredientView ingredientView = Instantiate(_playerIngredientView, _ingredientViewHolder);
			ingredientView.Initialize(ingredient);
		}
	}
	
	public void CraftButton()
	{
		if(Discovered)
		{
			// Crafting functionality here
			// If the player can even craft the recipe
			if(CanCraft())
			{
				// If the player's mouse item has an item
				if(_po.PlayerInventory.MouseItem.MouseInventoryItem.HasItem())
				{
					// If the player's mouse item is the same item
					if(_po.PlayerInventory.MouseItem.MouseInventoryItem.Item.Name == _recipe.Recipe.OutputItem.Name)
					{
						// Note to future self: Refactor this process later on
						_po.PlayerInventory.MouseItem.MouseInventoryItem.Quantity += _recipe.Recipe.OutputAmount;
						TakeItemsFromPlayer();
						_po.PlayerInventory.MouseItemView.UpdateView(_po.PlayerInventory.MouseItem.MouseInventoryItem);
						UpdateIngredientView();
					}
					else
					{
						// If it is not the same item, do nothing
						return;
					}
				}
				else
				{
					TakeItemsFromPlayer();
					CraftItem();
					UpdateIngredientView();
				}
			}
		}
	}
	
	private void TakeItemsFromPlayer()
	{
		// Take the items out of the player's inventory.
		foreach (InventoryItem ingredient in _recipe.Recipe.ResourceList)
		{
			_po.PlayerInventory.InventoryModel.RemoveItem(ingredient.Item, ingredient.Quantity);
		}
	}
	
	private void CraftItem()
	{
		// Give player the item
		_po.PlayerInventory.MouseItem.MouseInventoryItem.Item = _recipe.Recipe.OutputItem;
		_po.PlayerInventory.MouseItem.MouseInventoryItem.Quantity = _recipe.Recipe.OutputAmount;
		_po.PlayerInventory.MouseItemView.UpdateView(_po.PlayerInventory.MouseItem.MouseInventoryItem);
	}
	
	private bool CanCraft()
	{
		foreach (InventoryItem ingredient in _recipe.Recipe.ResourceList)
		{
			int inventoryAmount = _po.PlayerInventory.InventoryModel.GetAmount(ingredient.Item);
			int requiredAmount = ingredient.Quantity;
			
			if(requiredAmount > inventoryAmount)
			{
				return false;
			}
		}
		
		return true;
	}
	
	public void SetDiscoveredState()
	{
		_background.color = Color.cyan;
		_outputImage.color = Color.white;
	}
	
	public void SetNotDiscoveredState()
	{
		_background.color = Color.gray;
		_outputImage.color = Color.black;
	}
}
