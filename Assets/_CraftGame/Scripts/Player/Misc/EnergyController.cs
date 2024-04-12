using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
	[SerializeField] private int _maxEnergy;
	
	private EnergyView _energyView;
	private int _currentEnergy;
	
	private void Awake()
	{
		_currentEnergy = _maxEnergy;
		
		GameSignals.CLICKABLE_CLICKED.AddListener(DrainEnergy);
	}
	
	private void OnDestroy()
	{
		GameSignals.CLICKABLE_CLICKED.RemoveListener(DrainEnergy);
	}
	
	private void Start()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_energyView = FindObjectOfType<EnergyView>();
	}
	
	private void DrainEnergy(ISignalParameters parameters)
	{
		_currentEnergy--;
		_energyView.UpdateEnergy(_currentEnergy, _maxEnergy);
	}
}
