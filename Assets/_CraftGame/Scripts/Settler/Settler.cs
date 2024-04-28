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
	[SerializeField] private SettlerDataObject _settlerData;
	[SerializeField] private MMF_Player _rewardFeedback;
	[SerializeField] List<QuestObject> _questList = new(); // Index = expansion index
	
	private Image _itemDisplaySr;
	private Canvas _selectedCanvas;
	private TextMeshProUGUI _questText, _quotaText;
	private QuestObject _currentQuest;
	private bool _selected, _complete;
	private int _currentAmount, _questIndex = 0;

	private void Awake()
	{
		_itemDisplaySr = transform.GetChild(1).GetChild(2).GetComponent<Image>();
		_selectedCanvas = transform.GetChild(1).GetComponent<Canvas>();
		_questText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
		_quotaText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
		_settlerData.Setter = this;
		
		GameSignals.ON_EXPAND.AddListener(InjectQuestIndex);
	}
	
	private void OnDestroy()
	{
		GameManager.Instance.UpdateSettlerCurrentAmount(_settlerData, _currentAmount);
		GameSignals.ON_EXPAND.RemoveListener(InjectQuestIndex);
	}
	
	private void Start()
	{
		InjectQuestIndex(null);
	}
	
	private void InjectQuestIndex(ISignalParameters parameters)
	{
		_questIndex = GameManager.Instance.GetQuestIndex(_settlerData);
		Debug.Log("Quest Index: " + _questIndex);
		if(_questIndex > _questList.Count - 1)
		{
			// Debug.LogError("QuestIndex not found");
			return;
		}
		
		_currentQuest = _questList[_questIndex];
		_itemDisplaySr.sprite = _currentQuest.ItemNeeded.UiDisplay;
		_currentAmount = GameManager.Instance.GetQuestCurrentAmount(_settlerData);
		_complete = false;
		UpdateHoverText();
		UpdateQuotaText();
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
				
				GameManager.Instance.UpdateSettlerCurrentAmount(_settlerData, _currentAmount);
				
				Signal signal = GameSignals.ON_QUEST_COMPLETE;
				signal.ClearParameters();
				signal.AddParameter("SettlerData", _settlerData);
				signal.Dispatch();
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
