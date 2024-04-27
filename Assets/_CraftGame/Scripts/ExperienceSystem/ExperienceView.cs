using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceView : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _xpText;
	[SerializeField] private TextMeshProUGUI _friendshipText;
	[SerializeField] private Button _expandButton;
	
	private int _currentExperience;
	private int _quotaExperience;
	private int _currentFriendship;
	private int _quotaFriendship;
	
	public void UpdateView(int currentExperience, int quotaExperience)
	{
		_currentExperience = currentExperience;
		_quotaExperience = quotaExperience; 
		
		if(_currentExperience < _quotaExperience)
		{
			_xpText.text = $"<color=white>• XP: {_currentExperience} / {_quotaExperience}";
		}
		else
		{
			_xpText.text = $"<color=green>• XP: {_currentExperience} / {_quotaExperience}";
		}
		
		_expandButton.interactable = (_currentExperience >= _quotaExperience) && (_currentFriendship >= _quotaFriendship);
	}
	
	public void UpdateFriendship(int currentFriendship, int quotaFriendship)
	{
		_currentFriendship = currentFriendship;
		_quotaFriendship = quotaFriendship;
		_friendshipText.enabled = _quotaFriendship > 0;
		
		if(_currentFriendship < _quotaFriendship)
		{
			_friendshipText.text = $"<color=white>• Quests: {_currentFriendship} / {_quotaFriendship}";
		}
		else
		{
			_friendshipText.text = $"<color=green>• Quests: {_currentFriendship} / {_quotaFriendship}";
		}
		
		_expandButton.interactable = (_currentExperience >= _quotaExperience) && (_currentFriendship >= _quotaFriendship);
	}
	
	public void OnExpandButtonClicked()
	{
		GameSignals.ON_EXPAND.Dispatch();
	}
}
