using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] private GameObject _itemBasePrefab;
	[SerializeField] private List<SettlerInfo> _settlerInfos;
	
	private int _currentExpansionIndex = 0;
	
	public int CurrentExpansionIndex => _currentExpansionIndex;
	
	[Serializable]
	public class SettlerInfo
	{
		public SettlerDataObject SetterData;
		public int QuestIndex;
	}
	
	private void Start()
	{
		GameSignals.ON_EXPAND.AddListener(OnExpansion);
		GameSignals.ON_QUEST_COMPLETE.AddListener(OnQuestComplete);
	}
	
	private void OnDestroy()
	{
		GameSignals.ON_EXPAND.RemoveListener(OnExpansion);
		GameSignals.ON_QUEST_COMPLETE.RemoveListener(OnQuestComplete);
	}
	
	private void OnExpansion(ISignalParameters parameters)
	{
		_currentExpansionIndex++;
	}
	
	private void OnQuestComplete(ISignalParameters parameters)
	{
		Debug.Log(parameters.HasParameter("SettlerData"));
		if(parameters.HasParameter("SettlerData"))
		{
			Debug.Log("GotQuest");
			var settlerData = (SettlerDataObject)parameters.GetParameter("SettlerData");
			TryToIncrementSettlerQuestIndex(settlerData);
		}
	}
	
	public int GetQuestIndex(SettlerDataObject settlerData)
	{
		foreach (var item in _settlerInfos)
		{
			if(item.SetterData == settlerData)
			{
				Debug.Log("Incrementing settler data");
				return item.QuestIndex;
			}
		}
		
		Debug.LogError($"{settlerData} quest index could not be recieved because this data could not befound in game manager list of settler data");
		return -1;
	}
	
	private void TryToIncrementSettlerQuestIndex(SettlerDataObject settlerData)
	{
		foreach (var item in _settlerInfos)
		{
			if(item.SetterData == settlerData)
			{
				item.QuestIndex++;
				return;
			}
		}
		
		Debug.LogError($"Could not find Settler to increment quest index for ({settlerData.name})");
	}
	
	public void SpawnItem(Vector2 worldPos, InventoryItem inventoryItem, bool playAudio = true)
	{
		GameObject newItemGo = Instantiate(_itemBasePrefab, worldPos, Quaternion.identity);
		ItemBehavior newItem = newItemGo.GetComponent<ItemBehavior>();

		if (playAudio)
		{
			// MMSoundManagerSoundPlayEvent.Trigger(_popSound, MMSoundManager.MMSoundManagerTracks.UI, default);
		}
		
		if (!inventoryItem.Item.Stackable || inventoryItem.Quantity <= 0)
		{
				inventoryItem.Quantity = 1;
		}
		
		newItem.Initialize(inventoryItem);
	}
}
