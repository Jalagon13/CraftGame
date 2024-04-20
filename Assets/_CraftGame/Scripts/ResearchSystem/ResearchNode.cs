using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ResearchNode : MonoBehaviour
{
	[SerializeField] private ItemObject _itemToResearch;
	[SerializeField] private int _xpCost;
	[SerializeField] private bool _discovered;
	[SerializeField] private List<ResearchNode> _nodesToDiscover = new();
	
	private SelectedResearchView _selectedResearchView;
	private Image _background;
	private Image _outputImage;
	private bool _isComplete;
	
	public bool IsComplete => _isComplete;
	public SelectedResearchView ResearchView { get { return _selectedResearchView;} set { _selectedResearchView = value; } }
	
	private void Awake()
	{
		_background = transform.GetChild(0).GetComponent<Image>();
		_outputImage = transform.GetChild(1).GetComponent<Image>();
		_outputImage.sprite = _itemToResearch.UiDisplay;
	}
	
	private void Start()
	{
		if(_discovered)
			SetDiscoveredState();
		else
			SetNotDiscoveredState();
	}
	
	private void SetCompleteState()
	{
		_background.color = Color.yellow;
		_outputImage.color = Color.white;
	}
	
	private void SetDiscoveredState()
	{
		_discovered = true;
		_background.color = Color.cyan;
		_outputImage.color = Color.white;
	}
	
	private void SetNotDiscoveredState()
	{
		_discovered = false;
		_background.color = Color.gray;
		_outputImage.color = Color.black;
	}
	
	public void DiscoverNode()
	{
		SetDiscoveredState();
	}
	
	public void OnNodeClick()
	{
		if(_discovered)
		{
			_selectedResearchView.Populate(this, _itemToResearch, _xpCost);
			_selectedResearchView.gameObject.SetActive(true);
		}
	}
	
	public void CompleteNode()
	{
		SetCompleteState();
		
		_isComplete = true;
		
		// Unlock all research nodes associated with this research node
		foreach (ResearchNode researchNode in _nodesToDiscover)
		{
			researchNode.DiscoverNode();
		}
	}
}
