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
		
		GameSignals.FOCUS_ITEM_UPDATED.AddListener(OnFocusInventoryItemUpdated);
	}
	
	private void OnDestroy()
	{
		GameSignals.FOCUS_ITEM_UPDATED.RemoveListener(OnFocusInventoryItemUpdated);
	}
	
	private void Update()
	{
		Vector2 parentPos = transform.parent.transform.position;
		Vector2 indicatorPosition = new(Mathf.FloorToInt(parentPos.x), Mathf.FloorToInt(parentPos.y));
		transform.position = indicatorPosition + new Vector2(0.5f, 0.5f);
	}
	
	private void OnFocusInventoryItemUpdated(ISignalParameters parameters)
	{
		if(!parameters.HasParameter("FocusItem")) return;
		
		_focusItem = (InventoryItem)parameters.GetParameter("FocusItem");
		
		if(_focusItem == null) return;
		
		
		if(_focusItem.Item == null || _focusItem == null)
		{
			HideIndicator();
			return;
		}
		
		if(_focusItem.Item is DeployObject || _focusItem.Item is BuildObject || _focusItem.Item is SettlerObject)
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
