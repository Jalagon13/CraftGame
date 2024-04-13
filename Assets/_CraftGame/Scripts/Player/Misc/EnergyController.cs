using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private int _maxEnergy;
	
	private EnergyView _energyView;
	private int _currentEnergy;
	
	private void Awake()
	{
		_currentEnergy = _maxEnergy;
		
		GameSignals.CLICKABLE_CLICKED.AddListener(DrainEnergy);
		GameSignals.ON_CONSUME.AddListener(AddEnergy);
	}
	
	private void OnDestroy()
	{
		GameSignals.CLICKABLE_CLICKED.RemoveListener(DrainEnergy);
		GameSignals.ON_CONSUME.RemoveListener(AddEnergy);
	}
	
	private void Start()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_energyView = FindObjectOfType<EnergyView>();
	}
	
	private void AddEnergy(ISignalParameters parameters)
	{
		int energyValue = (int)parameters.GetParameter("EnergyValue");
		ItemObject consumeObject = (ConsumableObject)parameters.GetParameter("ConsumeItem");
		
		_currentEnergy += energyValue;
		
		if(_currentEnergy >= _maxEnergy)
		{
			_currentEnergy = _maxEnergy;
		}
		
		_po.PlayerInventory.RemoveItem(consumeObject, 1);
		UpdateView();
	}
	
	private void DrainEnergy(ISignalParameters parameters)
	{
		_currentEnergy--;
		UpdateView();
	}
	
	private void UpdateView()
	{
		_energyView.UpdateEnergy(_currentEnergy, _maxEnergy);
	}
}
