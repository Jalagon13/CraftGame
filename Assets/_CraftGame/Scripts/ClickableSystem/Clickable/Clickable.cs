using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Clickable : MonoBehaviour
{
	[SerializeField] private string _clickableName;
	[SerializeField] private int _maxHitPoints;
	[SerializeField] private ToolType _breakType;
	[SerializeField] private bool _canBeHit = true;
	[SerializeField] private bool _canGiveXp = true;
	[SerializeField] private LootTable _lootTable;
	[Header("Feedbacks")]
	[SerializeField] private MMF_Player _clickFeedback;
	[SerializeField] private MMF_Player _destroyFeedback;
	[SerializeField] private MMF_Player _spawnFeedback;
	
	private bool _selected;
	private int _currentHitPoints;
	private Vector2 _dropPosition;
	
	public string Name => _clickableName;
	
	private void Awake()
	{
		_currentHitPoints = _maxHitPoints;
		_dropPosition = (Vector2)transform.position + (Vector2.one * 0.5f);
	}
	
	private void Start()
	{
		_spawnFeedback?.PlayFeedbacks();
	}
	
	public void OverrideLootTable(LootTable lootTable)
	{
		_lootTable = lootTable;
	}
	
	public void Hit(int amount, ToolType incomingToolType)
	{
		if(_breakType == ToolType.None)
			goto byPass;
		if(incomingToolType != _breakType) return;
		byPass:
		
		_clickFeedback?.PlayFeedbacks(_clickFeedback.transform.position, amount);
		_currentHitPoints -= amount;
		
		GameSignals.CLICKABLE_CLICKED.Dispatch();
		
		if(_currentHitPoints <= 0)
			Break();
	}
	
	public void Break()
	{
		_lootTable.SpawnLoot(_dropPosition);
		PlayDestroyFeedbacks();
		
		Signal signal = GameSignals.CLICKABLE_DESTROYED;
		signal.ClearParameters();
		if(_canGiveXp)
		{
			signal.AddParameter("Experience", _maxHitPoints);
		}
		signal.AddParameter("Clickable", this);
		signal.Dispatch();
		
		Destroy(gameObject);
	}
	
	private void PlayDestroyFeedbacks()
	{
		if (_destroyFeedback != null)
		{
			_destroyFeedback.transform.SetParent(null);
			_destroyFeedback?.PlayFeedbacks(_clickFeedback.transform.position, _maxHitPoints);
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
