using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseItemView : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private Image _itemImage;
	[SerializeField] private TextMeshProUGUI _itemQuantityText;
	
	private void Update()
	{
		if(Camera.main == null) return;
		
		UpdatePosition();
	}
	
	private void UpdatePosition()
	{
		transform.position = Camera.main.WorldToScreenPoint(_po.MousePosition);
	}
	
	public void Initialize()
	{
		_itemImage.color = new Vector4(1, 1, 1, 0);
		_itemImage.sprite = null;
		_itemQuantityText.text = string.Empty;
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
