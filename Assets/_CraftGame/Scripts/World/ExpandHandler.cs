using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

public class ExpandHandler : MonoBehaviour
{
	[SerializeField] private MMF_Player _expandFeedBacks;
	[SerializeField] private UnityEvent _onFinalLevelComplete;
	
	private int _numberOfLevels;
	private int _currentLevelIndex;
	
	private void Awake()
	{
		_numberOfLevels = transform.childCount;
		_currentLevelIndex = 0;
		SetLevel(_currentLevelIndex);
		
		GameSignals.ON_EXPAND.AddListener(OnExpand);
	}
	
	private void OnDestroy()
	{
		GameSignals.ON_EXPAND.RemoveListener(OnExpand);
	}
	
	private void OnExpand(ISignalParameters parameters)
	{
		_expandFeedBacks?.PlayFeedbacks();
		
		_currentLevelIndex++;
		if(_currentLevelIndex < transform.childCount)
		{
			SetLevel(_currentLevelIndex);
		}
		else
		{
			// After final level complete
			_onFinalLevelComplete?.Invoke();
		}
	}
	
	private void SetLevel(int index)
	{
		foreach (Transform item in transform)
		{
			item.gameObject.SetActive(false);
		}
		
		foreach (Transform item in transform)
		{
			if(item.GetSiblingIndex() == index)
			{
				item.gameObject.SetActive(true);
				return;
			}
		}
	}
}
