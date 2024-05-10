using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeView : MonoBehaviour
{
	[SerializeField] private Image _fillImage;
	[SerializeField] private RectTransform _dayTransitionPanel;
	
	public void Initialize()
	{
		_dayTransitionPanel.gameObject.SetActive(false);
	}
	
	public void UpdateTime(float maxTime, float currentTime)
	{
		_fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, currentTime, maxTime));
		
		// Test this next time you open this up and then make the XP stuff too
	}
	
	public void EndDay() // Connected to end day button
	{
		GameSignals.ON_DAY_END.Dispatch();
		
		_dayTransitionPanel.gameObject.SetActive(true);
	}
	
	public void StartDay() // Connected to start day button
	{
		GameSignals.ON_DAY_START.Dispatch();
		
		_dayTransitionPanel.gameObject.SetActive(false);
	}
}