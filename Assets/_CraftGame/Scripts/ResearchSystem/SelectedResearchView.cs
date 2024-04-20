using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedResearchView : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private TextMeshProUGUI _outputText;
	[SerializeField] private TextMeshProUGUI _xpText;
	[SerializeField] private Image _outputImage;
	[SerializeField] private Button _researchButton;
	
	private ItemObject _researchItem;
	private ResearchNode _selectedResearchNode;
	private int _xpCost;
	
	public void Populate(ResearchNode selectedResearchNode, ItemObject item, int xpCost)
	{
		_researchItem = item;
		_xpCost = xpCost;
		_selectedResearchNode = selectedResearchNode;
		
		UpdateView();
	}
	
	private void UpdateView()
	{
		_outputText.text = $"{_researchItem.Name}<br>Recipe";
		_xpText.text = $"XP: {_xpCost}";
		_outputImage.sprite = _researchItem.UiDisplay;
		
		
		// Write XP Check Functionality to determine if the button is interactable
		_researchButton.interactable = _po.PlayerExperience.ExperienceModel.CurrentValue >= _xpCost;
		_researchButton.gameObject.SetActive(!_selectedResearchNode.IsComplete);
	}
	
	public void ResearchButton()
	{
		// Send out some event or something with the ItemObject to some crafting database or something
		Signal signal = GameSignals.ON_RECIPE_RESEARCHED;
		signal.ClearParameters();
		signal.AddParameter("ResearchItem", _researchItem);
		signal.Dispatch();
		
		// Complete Selected Research Node
		_selectedResearchNode.CompleteNode();
		_po.PlayerExperience.SubtractExperience(_xpCost);
		
		UpdateView();
		
		// Play Research Game Feel
	}
}
