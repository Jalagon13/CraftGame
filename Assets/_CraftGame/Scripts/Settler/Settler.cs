using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Settler : MonoBehaviour, IInteractable
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private MMF_Player _rewardFeedback;
	[SerializeField] List<QuestObject> _questList = new(); // Index = expansion index
	
	private Image _itemDisplaySr;
	private Canvas _selectedCanvas;
	private TextMeshProUGUI _questText;
	private TextMeshProUGUI _quotaText;
	private QuestObject _currentQuest;
	private bool _selected;
	private bool _complete;
	private int _currentAmount;

	private void Awake()
	{
		_itemDisplaySr = transform.GetChild(1).GetChild(2).GetComponent<Image>();
		_selectedCanvas = transform.GetChild(1).GetComponent<Canvas>();
		_questText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
		_quotaText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
		
		GameSignals.ON_EXPAND.AddListener(SetCurrentQuest);
	}
	
	private void OnDestroy()
	{
		GameSignals.ON_EXPAND.RemoveListener(SetCurrentQuest);
	}
	
	private IEnumerator Start()
	{
		yield return StartCoroutine(Delay());
		UpdateHoverText();
		UpdateQuotaText();
	}
	
	private void SetCurrentQuest(ISignalParameters parameters)
	{
		StartCoroutine(Delay());
	}
	private IEnumerator Delay()
	{
		yield return new WaitForEndOfFrame();
		_currentQuest = _questList[GameManager.Instance.CurrentExpansionIndex];
		_itemDisplaySr.sprite = _currentQuest.ItemNeeded.UiDisplay;
		_currentAmount = 0;
	}
	
	public void OnInteract()
	{
		if(_complete) return;
		
		InventoryItem inventoryItem = new()
		{
			Item = _currentQuest.ItemNeeded,
			Quantity = 1
		};
		
		if(_po.PlayerInventory.InventoryModel.Contains(inventoryItem))
		{
			int inventoryAmount = _po.PlayerInventory.InventoryModel.GetAmount(_currentQuest.ItemNeeded);
			int addAmount = inventoryAmount;
			int amountNeeded = _currentQuest.QuantityQuota - _currentAmount;
			
			if(inventoryAmount > amountNeeded)
			{
				// Take away _current Amount from inventory
				addAmount = amountNeeded;
			}
			
			_currentAmount += addAmount;
			_po.PlayerInventory.RemoveItem(_currentQuest.ItemNeeded, addAmount);
			UpdateQuotaText();
			
			if(_currentAmount >= _currentQuest.QuantityQuota)
			{
				_complete = true;
				_po.PlayerExperience.AddExperience(_currentQuest.XpReward);
				_rewardFeedback?.PlayFeedbacks();
				
				GameSignals.ON_QUEST_COMPLETE.Dispatch();
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
		_questText.text = $"[R] Give Item: <br><color=orange>{_currentQuest.ItemNeeded.Name}";
	}
	
	private void UpdateQuotaText()
	{
		if(_currentAmount < _currentQuest.QuantityQuota)
		{
			_quotaText.text = $"<color=red>: {_currentAmount} / {_currentQuest.QuantityQuota}";
		}
		else
		{
			_quotaText.text = $"<color=green>: {_currentAmount} / {_currentQuest.QuantityQuota}";
		}
		
		
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
