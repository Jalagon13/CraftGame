using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Clickable : MonoBehaviour
{
	[SerializeField] private int _maxHitPoints;
	[SerializeField] private bool _canBeHit = true;
	
	private int _currentHitPoints;
	
	private void Awake()
	{
		_currentHitPoints = _maxHitPoints;
	}
	
	public void Hit(int amount)
	{
		_currentHitPoints -= amount;
		
		if(_currentHitPoints <= 0)
			Break();
	}
	
	public void Break()
	{
		
		
		Destroy(gameObject);
	}
}
