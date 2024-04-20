using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDownIndicator : MonoBehaviour
{
	private InventoryItem _focusItem;
	private SpriteRenderer _sr;
	
	private void Awake()
	{
		_sr = GetComponent<SpriteRenderer>();
		
		GameSignals.FOCUS_INVENTORY_ITEM_UPDATED.AddListener(OnFocusInventoryItemUpdated);
	}
	
	private void OnDestroy()
	{
		GameSignals.FOCUS_INVENTORY_ITEM_UPDATED.AddListener(OnFocusInventoryItemUpdated);
	}
	
	private void Update()
	{
		Vector2 parentPos = transform.parent.transform.position;
		Vector2 indicatorPosition = new(Mathf.FloorToInt(parentPos.x), Mathf.FloorToInt(parentPos.y));
		transform.position = indicatorPosition + new Vector2(0.5f, 0.5f);
	}
	
	private void OnFocusInventoryItemUpdated(ISignalParameters parameters)
	{
		_focusItem = (InventoryItem)parameters.GetParameter("FocusInventoryItem");
		
		if(_focusItem.Item == null)
		{
			HideIndicator();
			return;
		}
		
		if(_focusItem.Item is DeployObject)
		{
			ShowIndicator();
		}
		else
		{
			HideIndicator();
		}
	}
	
	private void ShowIndicator()
	{
		_sr = GetComponent<SpriteRenderer>();
		_sr.enabled = true;
	}
	
	private void HideIndicator()
	{
		_sr = GetComponent<SpriteRenderer>();
		_sr.enabled = false;
	}
}
