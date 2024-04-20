using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchView : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private SelectedResearchView _selectedResearchView;
	
	private bool _uiActive;
	
	private void Awake()
	{
		_selectedResearchView.gameObject.SetActive(false);
		
		// Find all Research Nodes and give them the right to populate the _selectedResearchView
		ResearchNode[] researchNodes = GetComponentsInChildren<ResearchNode>(true);
		
		foreach (ResearchNode rn in researchNodes)
		{
			rn.ResearchView = _selectedResearchView;
		}
	}
	
	private void Start()
	{
		LoopThroughChildElements(false);
	}
	
	public void ToggleView()
	{
		if(!_uiActive && _po.SomeUiActive) return;
		
		_uiActive = !_uiActive;
		_po.SomeUiActive = _uiActive;
		
		LoopThroughChildElements(_uiActive);
		
		if(_uiActive)
		{
			_selectedResearchView.gameObject.SetActive(false);
			Signal signal = GameSignals.ON_UI_ACTIVATED;
			signal.ClearParameters();
			signal.Dispatch();
		}
		else
		{
			Signal signal = GameSignals.ON_UI_UNACTIVED;
			signal.ClearParameters();
			signal.Dispatch();
		}
	}
	
	private void LoopThroughChildElements(bool enabled)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform childTransform = transform.GetChild(i);
			childTransform.gameObject.SetActive(enabled);
		}
	}
}
