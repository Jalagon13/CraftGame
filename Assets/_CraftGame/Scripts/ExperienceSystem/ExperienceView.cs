using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperienceView : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _xpText;
	
	public void UpdateView(int currentExperience)
	{
		_xpText.text = $"XP: {currentExperience}";
	}
}
