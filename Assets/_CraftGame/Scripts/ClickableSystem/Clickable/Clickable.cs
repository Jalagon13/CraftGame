using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Clickable : MonoBehaviour
{
	[SerializeField] private int _maxHitPoints;
	[SerializeField] private bool _canBeHit = true;
	[SerializeField] private LootTable _lootTable;
	[Header("Feedbacks")]
	[SerializeField] private MMF_Player _clickFeedback;
	[SerializeField] private MMF_Player _destroyFeedback;
	
	private bool _selected;
	private int _currentHitPoints;
	private Vector2 _dropPosition;
	
	private void Awake()
	{
		_currentHitPoints = _maxHitPoints;
		_dropPosition = (Vector2)transform.position + (Vector2.one * 0.5f);
	}
	
	public void Hit(int amount)
	{
		_clickFeedback?.PlayFeedbacks();
		_currentHitPoints -= amount;
		
		if(_currentHitPoints <= 0)
			Break();
	}
	
	public void Break()
	{
		_lootTable.SpawnLoot(_dropPosition);
		PlayDestroyFeedbacks();
		
		Destroy(gameObject);
	}
	
	private void PlayDestroyFeedbacks()
	{
		if (_destroyFeedback != null)
		{
			_destroyFeedback.transform.SetParent(null);
			_destroyFeedback?.PlayFeedbacks();
		}
	}
	
	private void Selected()
	{
		_selected = true;
	}
	
	private void UnSelected()
	{
		_selected = false;
	}
	
	#region Select Methods
	private void OnMouseOver()
	{
		Selected();
	}

	private void OnMouseExit()
	{
		UnSelected();
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.TryGetComponent(out CursorControl cc))
		{
			Selected();
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.TryGetComponent(out CursorControl cc))
		{
			UnSelected();
		}
	}
	#endregion
	
}
