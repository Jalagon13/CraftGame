using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class OfferTotem : MonoBehaviour, IInteractable
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private ItemObject _itemNeeded;
	[SerializeField] private int _quantityQuota;
	[SerializeField] private int _xpReward;
	[SerializeField] private MMF_Player _rewardFeedback;
	
	private SpriteRenderer _itemDisplaySr;
	private Canvas _selectedCanvas;
	private TextMeshProUGUI _expandText;
	private TextMeshProUGUI _quotaText;
	private bool _selected;
	private bool _complete;
	private int _currentAmount;


	private void Awake()
	{
		_itemDisplaySr = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
		_itemDisplaySr.sprite = _itemNeeded.UiDisplay;
		_selectedCanvas = transform.GetChild(1).GetComponent<Canvas>();
		_expandText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
		_quotaText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
		_currentAmount = _quantityQuota;
	}
	
	private void Start()
	{
		UpdateHoverText();
		UpdateQuotaText();
	}
	
	public void OnInteract()
	{
		if(_complete) return;
		
		InventoryItem inventoryItem = new()
		{
			Item = _itemNeeded,
			Quantity = 1
		};
		
		if(_po.PlayerInventory.InventoryModel.Contains(inventoryItem))
		{
			int inventoryAmount = _po.PlayerInventory.InventoryModel.GetAmount(_itemNeeded);
			int takeAway = inventoryAmount;
			
			if(inventoryAmount > _currentAmount)
			{
				// Take away _current Amount from inventory
				takeAway = _currentAmount;
			}
			
			_currentAmount -= takeAway;
			_po.PlayerInventory.RemoveItem(_itemNeeded, takeAway);
			UpdateQuotaText();
			
			if(_currentAmount <= 0)
			{
				_complete = true;
				_po.PlayerExperience.AddExperience(_xpReward);
				_rewardFeedback?.PlayFeedbacks();
			}
		}
	}
	
	private void Selected()
	{
		_selected = true;
		_selectedCanvas.gameObject.SetActive(true);
	}
	
	private void UnSelected()
	{
		_selected = false;
		_selectedCanvas.gameObject.SetActive(false);
	}
	
	private void UpdateHoverText()
	{
		_expandText.text = $"[R] Give Item: {_itemNeeded.Name}";
	}
	
	private void UpdateQuotaText()
	{
		_quotaText.text = $"{_currentAmount}";
	}
	
	#region Select Methods
	
	private void OnMouseOver()
	{
		Selected();
	}

	private void OnMouseExit()
	{
		UnSelected();
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.TryGetComponent(out CursorControl cc))
		{
			Selected();
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.TryGetComponent(out CursorControl cc))
		{
			UnSelected();
		}
	}

	
	#endregion
}
