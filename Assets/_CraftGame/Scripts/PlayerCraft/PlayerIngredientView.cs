using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIngredientView : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private TextMeshProUGUI _ingredientText;
	[SerializeField] private Image _ingredientImage;
	
	public void Initialize(InventoryItem ingredient)
	{
		int inventoryAmount = _po.PlayerInventory.InventoryModel.GetAmount(ingredient.Item);
		int requiredAmount = ingredient.Quantity;
		
		if(requiredAmount > inventoryAmount)
		{
			_ingredientText.text = $"<color=red>{ingredient.Item.Name}[{inventoryAmount}/{requiredAmount}]</color=red>";
		}
		else
		{
			_ingredientText.text = $"{ingredient.Item.Name}[{inventoryAmount}/{requiredAmount}]";
		}
		
		_ingredientImage.sprite = ingredient.Item.UiDisplay;
	}
}
