using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientView : MonoBehaviour
{
	[SerializeField] private Image _ingredientImage;
	[SerializeField] private TextMeshProUGUI _ingredientText;
	
	public void Initialize(ItemObject ingredientItem, int inventoryAmount, int requiredAmount)
	{
		_ingredientImage.sprite = ingredientItem.UiDisplay;
		
		if(inventoryAmount < requiredAmount)
		{
			_ingredientText.text = $"<color=red>{ingredientItem.Name} [{inventoryAmount}/{requiredAmount}]";
		}
		else
		{
			_ingredientText.text = $"<color=white>{ingredientItem.Name} [{inventoryAmount}/{requiredAmount}]";
		}
	}
}
