using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// I need Craft Amount, Craft Timer Remaining Seconds, Craft Timer Remaining Seconds fraction for Fill Image
public class ProgressView : MonoBehaviour
{
	[SerializeField] private Image _fillImage;
	[SerializeField] private TextMeshProUGUI _craftAmountText;
	[SerializeField] private TextMeshProUGUI _secondsLeftText;
	
	private CraftingController _craftingController;
	
	private void Update()
	{
		
	}
	
	public void TimerFillImage()
	{
		// _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, currentEnergy, maxEnergy));
	}
	
	public void StartProcessViewUI()
	{
		
	}
	
	public void StopProcessViewUI()
	{
		
	}
}
