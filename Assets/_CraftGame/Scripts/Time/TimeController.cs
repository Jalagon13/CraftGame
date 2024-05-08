using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;



public class TimeController
 : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private int _maxTime;
	
	private TimeView _timeView;
	private int _currentTime;
	
	private void Awake()
	{
		_currentTime = 0;
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(IncrementTime);
	}
	
	private void OnDestroy()
	{
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(IncrementTime);
	}
	
	private void Start()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_timeView = FindObjectOfType<TimeView>();
		UpdateView();
	}
	
	private void IncrementTime(ISignalParameters parameters)
	{
		if(!parameters.HasParameter("TimeAmount")) return;
		
		int timeAmount = (int)parameters.GetParameter("TimeAmount");
		_currentTime += timeAmount;
		
		if(_currentTime > _maxTime)
		{
			_currentTime = _maxTime;
			UpdateView();
			// End of day stuff here
		}
		
		UpdateView();
	}
	
	private void UpdateView()
	{
		_timeView.UpdateTime(_currentTime, _maxTime);
	}
}
