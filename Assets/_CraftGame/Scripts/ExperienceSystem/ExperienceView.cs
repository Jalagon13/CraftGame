using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceView : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _xpText;
	[SerializeField] private Button _expandButton;
	
	public void UpdateView(int currentExperience, int quotaExperience)
	{
		_xpText.text = $"XP: {currentExperience} / {quotaExperience}";
		_expandButton.interactable = currentExperience >= quotaExperience;
	}
	
	public void OnExpandButtonClicked()
	{
		GameSignals.ON_EXPAND.Dispatch();
	}
}
