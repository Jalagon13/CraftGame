using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;



public class TimeController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private float _maxTime;
	
	private TimeView _timeView;
	private float _currentTime;
	public DayCycleHandler DayCycleHandler { get; set; }
	public float CurrentDayRatio => _currentTime / _maxTime;
	
	private void Awake()
	{
		_po.TimeController = this;
		_currentTime = 0;
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(IncrementTime);
		GameSignals.ON_DAY_END.AddListener(OnDayEnd);
		GameSignals.ON_DAY_START.AddListener(OnDayStart);
	}
	
	private void OnDestroy()
	{
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(IncrementTime);
		GameSignals.ON_DAY_END.RemoveListener(OnDayEnd);
		GameSignals.ON_DAY_START.RemoveListener(OnDayStart);
	}
	
	private IEnumerator Start()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_timeView = FindObjectOfType<TimeView>();
		_timeView.Initialize();
		UpdateView();
		
		yield return new WaitForEndOfFrame();
		DayCycleHandler.Tick();
	}
	
	public bool NoMoreEnergy()
	{
		return _currentTime >= _maxTime;
	}
	
	private void IncrementTime(ISignalParameters parameters)
	{
		if(!parameters.HasParameter("TimeAmount")) return;
		
		int timeAmount = (int)parameters.GetParameter("TimeAmount");
		_currentTime += timeAmount;
		DayCycleHandler.Tick();
		
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
	
	private void OnDayStart(ISignalParameters parameters)
	{
		
	}
	
	private void OnDayEnd(ISignalParameters parameters)
	{
		
	}
}
