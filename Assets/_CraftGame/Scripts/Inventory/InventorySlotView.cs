using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Image _itemImage;
	[SerializeField] private TextMeshProUGUI _itemQuantityText;
	
	private int _inventoryIndex;

	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			Signal signal = GameSignals.ON_SLOT_LEFT_CLICKED;
			signal.ClearParameters();
			signal.AddParameter("inventoryIndex", _inventoryIndex);
			signal.Dispatch();
		}
		else if(eventData.button == PointerEventData.InputButton.Right)
		{
			Signal signal = GameSignals.ON_SLOT_RIGHT_CLICKED;
			signal.ClearParameters();
			signal.AddParameter("inventoryIndex", _inventoryIndex);
			signal.Dispatch();
		}
	}
	
	public void Initialize(int inventoryIndex)
	{
		_inventoryIndex = inventoryIndex;
	}

	public void UpdateView(InventoryItem item)
	{
		if(item.Item != null)
		{
			_itemImage.color = new Vector4(1, 1, 1, 1);
			_itemImage.sprite = item.Item.UiDisplay;
		}
		else
		{
			_itemImage.color = new Vector4(1, 1, 1, 0);
			_itemImage.sprite = null;
		}
		
		_itemQuantityText.text = item.Item != null ? item.Quantity.ToString() : string.Empty;
	}
}
