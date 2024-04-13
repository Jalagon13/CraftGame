using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
	[SerializeField] private PlayerObject _po;
	private ExperienceView _experienceView;
	
	private int _currentExperience;
	
	public int CurrentExperience { get { return _currentExperience; } 
		set 
		{ 
			_currentExperience = value; 
			_experienceView.UpdateView(_currentExperience);
		} 
	} 
	
	private void Awake()
	{
		_po.PlayerExperience = this;
		
		GameSignals.CLICKABLE_DESTROYED.AddListener(OnClickableDestroyed);
	}
	
	private void OnDestroy()
	{
		GameSignals.CLICKABLE_DESTROYED.RemoveListener(OnClickableDestroyed);
	}
	
	private void Start()
	{
		// Future note to self: This may cause some issues when creating a scene loading bootstrap
		_experienceView = FindObjectOfType<ExperienceView>();
		_experienceView.UpdateView(0);
	}
	
	private void OnClickableDestroyed(ISignalParameters parameters)
	{
		int xpFromClickable = (int)parameters.GetParameter("Experience");
		
		_currentExperience += xpFromClickable;
		_experienceView.UpdateView(_currentExperience);
	}
}
