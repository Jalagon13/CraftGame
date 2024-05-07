using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeView : MonoBehaviour
{
	[SerializeField] private Image _fillImage;
	
	public void UpdateTime(int maxEnergy, int currentEnergy)
	{
		_fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, currentEnergy, maxEnergy));
		
		// Test this next time you open this up and then make the XP stuff too
	}
}