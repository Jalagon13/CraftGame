using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ProgressionTotem : MonoBehaviour, IInteractable
{
	[SerializeField] private PlayerObject _po;
	[SerializeField] private MMF_Player _expandFeedbacks;
	[Space()]
	[SerializeField] private List<ExpandData> _expandData;
	
	private Canvas _selectedCanvas;
	private TextMeshProUGUI _expandText;
	private bool _selected;
	private int _totemXp;
	private int _expandDataIndex;
	
	[Serializable]
	public class ExpandData
	{
		public int XpQuota;
		public UnityEvent OnQuotaMet;
	}
	
	private void Awake()
	{
		_selectedCanvas = transform.GetChild(1).GetComponent<Canvas>();
		_expandText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
	}
	
	private void Start()
	{
		UpdateExpandText();
		
		_selectedCanvas.gameObject.SetActive(false);
	}
	
	public void OnInteract()
    {
        int currentPlrXp = _po.PlayerExperience.ExperienceModel.CurrentValue;
		int currentXpQuota = _expandData[_expandDataIndex].XpQuota;
		int xpNeeded = currentXpQuota - _totemXp;
		
		if(currentPlrXp >= xpNeeded)
		{
			_totemXp += xpNeeded;
			_po.PlayerExperience.SubtractExperience(xpNeeded);
		}
		else
		{
			_totemXp += currentPlrXp;
			_po.PlayerExperience.ExperienceModel.CurrentValue = 0;
		}
		
		if(_totemXp >= _expandData[_expandDataIndex].XpQuota)
		{
			_expandData[_expandDataIndex].OnQuotaMet?.Invoke();
			_expandDataIndex++;
			_totemXp = 0;
			_expandFeedbacks?.PlayFeedbacks();
			_selectedCanvas.gameObject.SetActive(false);
		}
		
		UpdateExpandText();
    }
	
	private void UpdateExpandText()
	{
		_expandText.text = $"[R] Expand<br>XP: {_totemXp}/{_expandData[_expandDataIndex].XpQuota}";
	}
	
	private void Selected()
	{
		_selected = true;
		_selectedCanvas.gameObject.SetActive(true);
	}
	
	private void UnSelected()
	{
		_selected = false;
		_selectedCanvas.gameObject.SetActive(false);
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
